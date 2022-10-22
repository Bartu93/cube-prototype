using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarMoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(1f, .6f).OnComplete(Move);
    }

    private void Move()
    {
        transform.DOMove(UIManager.ins.starTextTransform.transform.position,.6f).OnComplete(AfterMoved);
    }

    void AfterMoved()
    {
        transform.DOScale(0f, .3f);
        Destroy(gameObject, 1f);
    }

}
