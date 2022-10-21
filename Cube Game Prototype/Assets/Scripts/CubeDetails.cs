using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeDetails : MonoBehaviour
{
    public Vector3 previousPosition;
    public int cubeID;
    public bool sentToDeck;
    Transform parent;

    private void Awake()
    {
        StartCoroutine(GetPositionAndParentDelay());
    }

    public void MoveToDeck()
    {

        if(Deck.ins.deckhasavailablespace == true && sentToDeck == false)
        {
            Deck.ins.cubesInDeck.Add(gameObject);
            Deck.ins.SortDeckCubes();
            transform.SetParent(Deck.ins.transform);
            sentToDeck = true;
        }
    }

    IEnumerator GetPositionAndParentDelay()
    {
        yield return new WaitForSeconds(.5f);
        previousPosition = transform.position;
        parent = transform.parent;
    }

    public void MoveBackToPosition()
    {
        transform.DOMove(previousPosition, 1f);
        sentToDeck = false;
        transform.DOScale(1f, 1f);
        transform.SetParent(parent);
    }

    public void DestroyCube()
    {
        transform.DOScale(0, .6f);
        Destroy(gameObject, 1f);
    }
}
