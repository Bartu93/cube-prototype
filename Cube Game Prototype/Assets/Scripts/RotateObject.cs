using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    public Vector3 mPrevPos = Vector3.zero;
    public Vector3 mPosDelta = Vector3.zero;

    public float rotSpeed = 1f;
    public Camera mainCamera;
    public bool hitCube;
    public bool rotated;

    private void Start()
    {
        mainCamera = Camera.main;
        transform.DOMove(new Vector3(0, 0, 2),1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mPrevPos = Input.mousePosition;
            rotated = false;
        }

        if (Input.GetMouseButton(0))
        {
            {
                if(mPrevPos != Input.mousePosition)
                {
                    rotated = true;
                }
                mPosDelta = (Input.mousePosition - mPrevPos).normalized * rotSpeed;

                if (Vector3.Dot(transform.up, Vector3.up) >= 0)
                {
                    transform.Rotate(transform.up, -Vector3.Dot(mPosDelta * rotSpeed, Camera.main.transform.right), Space.World);
                }
                else
                {
                    transform.Rotate(transform.up, Vector3.Dot(mPosDelta * rotSpeed, Camera.main.transform.right), Space.World);
                }

                transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta * rotSpeed, Camera.main.transform.up), Space.World);
            }

            mPrevPos = Input.mousePosition;

        }

        if (Input.GetMouseButtonUp(0) == true && rotated == false)
        {
            mPrevPos = Vector3.zero;

            //if (rotating == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.CompareTag("Cube"))
                    {
                        hitCube = true;
                        //if ((Input.mousePosition - mPrevPos).normalized.sqrMagnitude < 1f)
                        {
                            hit.transform.GetComponent<CubeDetails>().MoveToDeck();
                        }
                    }
                }
                else
                {
                    hitCube = false;
                }
            }

            rotated = false;
        }

    }
}
