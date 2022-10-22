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
        obj.GetComponent<CubeDetails>().MoveBackToPosition();
    }

    public void HintPowerUp()
    {
        AvailableCubesForHint.Sort(CompareListByID);

        if (cubesInDeck.Count == 2)
        {
            int cubeID = cubesInDeck[cubesInDeck.Count - 1].GetComponent<CubeDetails>().cubeID;
            Debug.Log(cubeID);
            SortAvailableHintCubes(0, cubeID, 2);
        }
        else if (cubesInDeck.Count == 1)
        {
            int cubeID = cubesInDeck[cubesInDeck.Count - 1].GetComponent<CubeDetails>().cubeID;
            Debug.Log(cubeID);
            SortAvailableHintCubes(0, cubeID, 2);
        }
        else if (cubesInDeck.Count == 0)
        {
            SortAvailableHintCubes(0,1,3);
        }
    }

    void SortAvailableHintCubes(int index, int cubeID, int countNumber)
    {
        if (AvailableCubesForHint.Count > 2)
        {
            //var cubeID = AvailableCubesForHint[index].GetComponent<CubeDetails>().cubeID;


            var count = 1;
            for (int i = 1; i < AvailableCubesForHint.Count; i++)
            {
                if (AvailableCubesForHint[i].GetComponent<CubeDetails>().cubeID == cubeID)
                {
                    count++;
                    if(count == countNumber)
                    {
                        switch (countNumber)
                        {
                            case 3:
                                AvailableCubesForHint[i].GetComponent<CubeDetails>().MoveToDeck();
                                AvailableCubesForHint.Remove(AvailableCubesForHint[i]);
                                AvailableCubesForHint[i - 1].GetComponent<CubeDetails>().MoveToDeck();
                                AvailableCubesForHint.Remove(AvailableCubesForHint[i - 1]);
                                AvailableCubesForHint[i - 2].GetComponent<CubeDetails>().MoveToDeck();
                                AvailableCubesForHint.Remove(AvailableCubesForHint[i - 2]);
                                break;

                            //case 2:
                            //    AvailableCubesForHint[i].GetComponent<CubeDetails>().MoveToDeck();
                            //    AvailableCubesForHint.Remove(AvailableCubesForHint[i]);
                            //    AvailableCubesForHint[i - 1].GetComponent<CubeDetails>().MoveToDeck();
                            //    AvailableCubesForHint.Remove(AvailableCubesForHint[i - 1]);
                            //    break;

                            //case 1:
                            //    AvailableCubesForHint[i].GetComponent<CubeDetails>().MoveToDeck();
                            //    AvailableCubesForHint.Remove(AvailableCubesForHint[i]);
                            //    break;
                        }
                        break;
                    }
                    

                    



                    //if (count == countNumber)
                    //{
                        
                    //    //StartCoroutine(RemoveMatchingCubes());
                    //}

                    //if (count == countNumber)
                    //{
                    //    AvailableCubesForHint[i].GetComponent<CubeDetails>().MoveToDeck();
                    //    AvailableCubesForHint.Remove(AvailableCubesForHint[i]);
                    //    AvailableCubesForHint[i - 1].GetComponent<CubeDetails>().MoveToDeck();
                    //    AvailableCubesForHint.Remove(AvailableCubesForHint[i - 1]);
                    //    break;
                    //    //StartCoroutine(RemoveMatchingCubes());
                    //}

                    
                }
                else
                {
                    cubeID = AvailableCubesForHint[i].GetComponent<CubeDetails>().cubeID;
                    count = 1;
                }

            }
        }
    }

}
