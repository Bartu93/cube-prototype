using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public GameObject spawnPrefab;
    public GameObject rotatorPrefab;
    public Transform parent;
    
    public List<Cube> cubes;
    public List<GameObject> cubeGameObjects;
    public List<Vector3> spawnablePositions;

    public float xMaxValue;
    public int yMaxValue;
    public int zMaxValue;

    public int totalCubeNumber;
    public int maxCubeNumber;

    public bool debugMode;
    

    Vector3 spawnPos;

    void Start()
    {
        parent = this.transform;     
        
        for (int y = 0; y < yMaxValue; y++)
        {
            //Calculates all available X & Z positions on the same Y axis then moves to the next Y axis if there is none left
            CalculateXZAxis();
            spawnPos.y++;
        }

        //For debugging purposes
        if (debugMode)
        {
            foreach (var item in spawnablePositions)
            {
                GameObject obj = Instantiate(spawnPrefab, item, Quaternion.identity,parent.transform);
                cubeGameObjects.Add(obj);
            }
        }

        InstantiateCubesRandomly();
    }

    void OffsetInstantiationPosition()
    {
        //Offsets positions of cubes to center all cubes
        parent.transform.position = Vector3.zero;

        //Offsets according to divisibility to 2 or not  
        if (xMaxValue % 2 == 0)
        {
            float offsetX = (-xMaxValue / 2) + .5f;
            parent.transform.position = new Vector3(parent.transform.position.x + offsetX, parent.transform.position.y + parent.transform.position.z);
        }

        if (xMaxValue % 2 != 0)
        {
            float offsetX = (-xMaxValue / 2) + .5f;
            parent.transform.position = new Vector3(parent.transform.position.x + offsetX, parent.transform.position.y + parent.transform.position.z);
        }

        GetCentralPositionofAllObjects();
    }

    void GetCentralPositionofAllObjects()
    {
        //Gets the centre of all spawned cubes for rotation purposes

        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;
        foreach (var item in cubeGameObjects)
        {
            totalX += item.transform.position.x;
            totalY += item.transform.position.y;
            totalZ += item.transform.position.z;
        }
        var centerX = totalX / cubeGameObjects.Count;
        var centerY = totalY / cubeGameObjects.Count;
        var centerZ = totalZ / cubeGameObjects.Count;

        //Spawns a rotator at the centre of all cubes
        GameObject rotator = Instantiate(rotatorPrefab, new Vector3(centerX, centerY, centerZ), Quaternion.identity);
        foreach (var item in cubeGameObjects)
        {
            item.transform.SetParent(rotator.transform);
        }
    }

    void CalculateXZAxis()
    {
        // Calculates all available X & Z spawning positions on the same Y axis
        for (int x = 0; x < xMaxValue; x++)
        {
            spawnablePositions.Add(spawnPos);

            for (int z = 0; z < zMaxValue; z++)
            {
                spawnPos.z++;
                spawnablePositions.Add(spawnPos);

                if (spawnPos.z == zMaxValue)
                {
                    spawnablePositions.Remove(spawnPos);
                    spawnPos.z = 0;
                }
            }
            spawnPos.x++;
            if (spawnPos.x == xMaxValue)
            {
                spawnablePositions.Remove(spawnPos);
                spawnPos.x = 0;
            }
        }
    }

    public void InstantiateCubesRandomly()
    {
            
        for (int i = 0; i < maxCubeNumber; i++)
            {
                if (totalCubeNumber < maxCubeNumber)
                {
                    int randomNumber = Random.Range(0, spawnablePositions.Count);
                    Vector3 spawnpos = spawnablePositions[randomNumber];
                    GameObject cube = Instantiate(spawnPrefab, spawnpos, Quaternion.identity, parent.transform);
                    cubeGameObjects.Add(cube);
                    totalCubeNumber++;
                    spawnablePositions.Remove(spawnpos);
                } 
            }
       
        OffsetInstantiationPosition();
    }
}
