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
    Vector3 previousrotation;

    private void Awake()
    {
        StartCoroutine(GetPositionAndParentDelay());
    }

    public void MoveToDeck()
    {
        Deck.ins.CheckAvailability();

        if (Deck.ins.deckhasavailablespace == true && sentToDeck == false)
        {
            Deck.ins.cubesInDeck.Add(gameObject);
            Deck.ins.SortDeckCubes();
            Deck.ins.SortVisualDeck();
            Deck.ins.CheckMatchingCubes();
            transform.SetParent(Deck.ins.transform);
            sentToDeck = true;
        }
    }

    IEnumerator GetPositionAndParentDelay()
    {
        yield return new WaitForSeconds(.5f);
        previousPosition = transform.localPosition;
        parent = transform.parent;
        previousrotation = transform.localRotation.eulerAngles;
    }

    public void MoveBackToPosition()
    {
        SetParent();
        transform.DOLocalMove(previousPosition, .2f);
        sentToDeck = false;
        transform.DOScale(1f, .2f);
        transform.DOLocalRotate(previousrotation, .2f);
        SetParent();
    }

    void SetParent()
    {
        transform.SetParent(parent);
    }

    public void DestroyCube()
    {
        transform.DOScale(0, .6f);
        GameManager.ins.assignedCubes.Remove(gameObject);
        GameManager.ins.totalCubeNumber--;
        Destroy(gameObject, 1f);
    }
}
