using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Deck : MonoBehaviour
{
    public List<Transform> transforms;
    public bool deckhasavailablespace;
    public List<GameObject> cubesInDeck;
    public List<GameObject> matchingList;
    public List<GameObject> AvailableCubesForHint;

    private static Deck _ins;
    public static Deck ins
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

        CheckAvailability();
    }

    public void CheckAvailability()
    {
        if(cubesInDeck.Count < 7)
        {
            deckhasavailablespace = true;
        }
        else if (cubesInDeck.Count == 7)
        {
            deckhasavailablespace = false;
        }
    }
    private static int CompareListByID(GameObject i1, GameObject i2)
    {
        return i1.GetComponent<CubeDetails>().cubeID.CompareTo(i2.GetComponent<CubeDetails>().cubeID);
    }

    public void SortDeckCubes()
    {
        cubesInDeck.Sort(CompareListByID);
    }

    public void SortVisualDeck()
    {
        foreach (var item in cubesInDeck)
        {
            int index = cubesInDeck.IndexOf(item);
            item.transform.DOMove(transforms[index].transform.position, .3f);
            item.transform.DOScale(1.2f,.3f);
            item.transform.DORotate(Vector3.zero,.3f);
            AvailableCubesForHint.Remove(item);
        }
        
    }

    public void CheckMatchingCubes()
    {
        if (cubesInDeck.Count > 2)
        {
            var cubeID = cubesInDeck[0].GetComponent<CubeDetails>().cubeID;
            var count = 1;
            for (int i = 1; i < cubesInDeck.Count; i++)
            {
                if (cubesInDeck[i].GetComponent<CubeDetails>().cubeID == cubeID)
                {
                    count++;
                    if (count == 3)
                    {
                        matchingList.Add(cubesInDeck[i]);
                        cubesInDeck.Remove(cubesInDeck[i]);
                        matchingList.Add(cubesInDeck[i-1]);
                        cubesInDeck.Remove(cubesInDeck[i-1]);
                        matchingList.Add(cubesInDeck[i-2]);
                        cubesInDeck.Remove(cubesInDeck[i-2]);
                        StartCoroutine(RemoveMatchingCubes());
                    }
                }
                else
                {
                    cubeID = cubesInDeck[i].GetComponent<CubeDetails>().cubeID;
                    count = 1;
                }

            }
        }
    }

    IEnumerator RemoveMatchingCubes()
    {
        yield return new WaitForSeconds(0.5f);
        matchingList[0].transform.DOMove(matchingList[1].transform.position, .3f);
        matchingList[2].transform.DOMove(matchingList[1].transform.position, .3f);
        for (int i = 0; i < matchingList.Count; i++)
        {
            matchingList[i].GetComponent<CubeDetails>().DestroyCube();
        }
        matchingList.Clear();
        
        if(GameManager.ins.totalCubeNumber == 0)
        {
            GameManager.ins.OnLevelCompleted();
        }
        CheckAvailability();
        SortDeckCubes();
        SortVisualDeck();
        UIManager.ins.OnCubesMatched();
    }

    public void UndoPowerUp()
    {
        if(cubesInDeck.Count == 0)
        {
            return;
        }
        GameObject obj = cubesInDeck[cubesInDeck.Count-1];
        cubesInDeck.Remove(obj);
        AvailableCubesForHint.Add(obj);
        obj.GetComponent<CubeDetails>().MoveBackToPosition();
    }

    public void HintPowerUp()
    {
        AvailableCubesForHint.Sort(CompareListByID);

        if (cubesInDeck.Count >= 1)
        {
            if(cubesInDeck.Count == transforms.Count - 1)
            {
                GameObject obj = cubesInDeck[cubesInDeck.Count - 1];
                cubesInDeck.Remove(obj);
                AvailableCubesForHint.Add(obj);
                obj.GetComponent<CubeDetails>().MoveBackToPosition();
            }
            int cubeID = cubesInDeck[cubesInDeck.Count - 1].GetComponent<CubeDetails>().cubeID;
            DeckHasCubesForHintPowerUp(cubeID);
        }
        else if (cubesInDeck.Count == 0)
        {
            int cubeID = AvailableCubesForHint[0].GetComponent<CubeDetails>().cubeID;
            DeckHasCubesForHintPowerUp(cubeID);
        }
    }

    void DeckHasCubesForHintPowerUp(int cubeID)
    {
        int count = 0;
        for (int i = 0; i < cubesInDeck.Count; i++)
        {
            if (cubesInDeck[i].GetComponent<CubeDetails>().cubeID == cubeID)
            {
                count++;
            }
        }
        
        count = 3 - count;
        SendCubesToDeckForHintPowerUp(count, cubeID);
        count = 0;
    }

    void SendCubesToDeckForHintPowerUp(int count, int cubeID)
    {
        GameObject availableHintCube;
        int i = 0;
        Debug.Log("initial i = " + i);
        Debug.Log("count = " + count);
        List<GameObject> cubesToBeSentToDeck = new List<GameObject>();

        for (int e = 0; e < AvailableCubesForHint.Count; e++)
        {
            if (AvailableCubesForHint[e].GetComponent<CubeDetails>().cubeID == cubeID)
            {
                if (i < count)
                {
                    availableHintCube = AvailableCubesForHint[e];
                    cubesToBeSentToDeck.Add(availableHintCube);
                    i++;
                }
            }
        }

        foreach (var item in cubesToBeSentToDeck)
        {
            item.GetComponent<CubeDetails>().MoveToDeck();
        }

        cubesToBeSentToDeck.Clear();
        
        
    }
}
