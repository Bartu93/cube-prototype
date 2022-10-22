using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPrefab;
    public GameObject rotatorPrefab;
    public Transform parent;
    
    public List<Cube> cubeScriptableObjects;
    public List<GameObject> unassignedCubes;
    public List<GameObject> assignedCubes;
    public List<Vector3> spawnablePositions;

    public int xMaxValue;
    public int yMaxValue;
    public int zMaxValue;

    public int totalCubeNumber;
    public int maxCubeNumber;

    public bool debugMode;

    public int undoPowerUpCount;
    public int hintPowerUpCount;

    public List<Level> levelScriptableObjects;
    public int levelNo;
    

    Vector3 spawnPos;

    private static GameManager _ins;
    public static GameManager ins
    {
        get
        {
            return _ins;
        }
    }

    void Awake()
    {
        if (_ins == null)
            _ins = this;
    }

    void Start()
    {
        parent = this.transform;

        InitLevel();
    }

    void InitLevel()
    {
        Level levelSO = levelScriptableObjects[levelNo];
        xMaxValue = levelSO.xMaxValue;
        yMaxValue = levelSO.yMaxValue;
        zMaxValue = levelSO.zMaxValue;
        maxCubeNumber = levelSO.maxCubeNumber;
        
        for (int y = 0; y < yMaxValue; y++)
        {
            //Calculates all available X & Z positions on the same Y axis then moves to the next Y axis if there is none left
            CalculateXZAxis();
            spawnPos.y++;
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
        foreach (var item in unassignedCubes)
        {
            totalX += item.transform.position.x;
            totalY += item.transform.position.y;
            totalZ += item.transform.position.z;
        }
        var centerX = totalX / unassignedCubes.Count;
        var centerY = totalY / unassignedCubes.Count;
        var centerZ = totalZ / unassignedCubes.Count;

        //Spawns a rotator at the centre of all cubes
        GameObject rotator = Instantiate(rotatorPrefab, new Vector3(centerX, centerY, centerZ), Quaternion.identity);
        foreach (var item in unassignedCubes)
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
                    unassignedCubes.Add(cube);
                    totalCubeNumber++;
                    spawnablePositions.Remove(spawnpos);
                } 
            }
       
        OffsetInstantiationPosition();

        AssignCubes();
    }

    public void AssignCubes()
    {
        int cubeMatchingNumber = 3;
        int cubeSOIndex = 0;
        int matchedCubeNumber = 0;
        int cubeGOCount = unassignedCubes.Count;

        for (int i = 0; i < cubeGOCount; i++)
        {
            if (cubeSOIndex > cubeScriptableObjects.Count - 1)
            {
                cubeSOIndex = 0;
            }
            Cube cubeSO = cubeScriptableObjects[cubeSOIndex];
            int randomNumber = Random.Range(0, unassignedCubes.Count);
            GameObject selectedCube = unassignedCubes[randomNumber];
            selectedCube.GetComponent<MeshRenderer>().material.mainTexture = cubeSO.texture;
            selectedCube.name = cubeSO.cubeID.ToString();
            selectedCube.GetComponent<CubeDetails>().cubeID = cubeSO.cubeID;
            unassignedCubes.Remove(selectedCube);
            assignedCubes.Add(selectedCube);
            matchedCubeNumber++;

            if(matchedCubeNumber == cubeMatchingNumber)
            {
                cubeSOIndex++;
                
                

                matchedCubeNumber = 0;
            }
        }
        Deck.ins.AvailableCubesForHint = assignedCubes;
    }

    public void OnLevelCompleted()
    {
        levelNo++;
        if(levelNo > levelScriptableObjects.Count - 1)
        {
            Debug.Log("Game Ended");
        }
        unassignedCubes.Clear();
        assignedCubes.Clear();
        spawnablePositions.Clear();
        InitLevel();
    }
}
