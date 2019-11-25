using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{        
    public GameObject background, pointer;

    private Touch oneTouch;
    private Vector2 touchPosition;
    private Vector2 moveDirection;
    private Vector3 dir;
    private float bgRadius;
    private float distance;

    private void Start()
    {
        background.SetActive(false);
        pointer.SetActive(false);
        bgRadius = background.gameObject.GetComponent<Transform>().localScale.y / 2;
    }

    private void Update()
    {
        //Debug.Log("반지름: " + bgRadius);

        if(Input.touchCount > 0)
        {
            oneTouch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(oneTouch.position);

            switch(oneTouch.phase)
            {
                case TouchPhase.Began:
                    background.SetActive(true);
                    pointer.SetActive(true);

                    background.transform.position = touchPosition;
                    pointer.transform.position = touchPosition;
                    break;
                case TouchPhase.Stationary:
                    SetPlayerDir();
                    break;
                case TouchPhase.Moved:
                    SetPlayerDir();
                    break;
                case TouchPhase.Ended:
                    background.SetActive(false);
                    pointer.SetActive(false);
                    moveDirection = Vector2.zero;
                    break;
            }
        }
    }

    private void SetPlayerDir()
    {
        pointer.transform.position = touchPosition;

        distance = Vector3.Distance(pointer.transform.position, background.transform.position);
        dir = (pointer.transform.position - background.transform.position).normalized;

        if (distance >= bgRadius)
        {
            pointer.transform.position = background.transform.position + (dir * bgRadius);
        }          
            /*
            pointer.transform.position = new Vector2(
                Mathf.Clamp(pointer.transform.position.x,
                background.transform.position.x - bgRadius, // 0.7f
                background.transform.position.x + bgRadius),
                Mathf.Clamp(pointer.transform.position.y,
                background.transform.position.y - bgRadius,
                background.transform.position.y + bgRadius));
                */       
        moveDirection = (pointer.transform.position - background.transform.position).normalized;        
    }

    public Vector2 GetPlayerDir()
    {
        // 조이스틱이 반지름의 1/3을 넘어가면 
        if (distance > (bgRadius / 3))
        {
            return moveDirection;        
        }
        else
        {
            return Vector2.zero;
        }
    }
}
