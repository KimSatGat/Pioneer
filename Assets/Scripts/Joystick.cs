using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{        
    public GameObject circle, dot;

    private Touch oneTouch;
    private Vector2 touchPosition;
    private Vector2 moveDirection;

    private void Start()
    {
        circle.SetActive(false);
        dot.SetActive(false);
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            oneTouch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(oneTouch.position);

            switch(oneTouch.phase)
            {
                case TouchPhase.Began:
                    circle.SetActive(true);
                    dot.SetActive(true);

                    circle.transform.position = touchPosition;
                    dot.transform.position = touchPosition;
                    break;
                case TouchPhase.Stationary:
                    SetPlayerDir();
                    break;
                case TouchPhase.Moved:
                    SetPlayerDir();
                    break;
                case TouchPhase.Ended:
                    circle.SetActive(false);
                    dot.SetActive(false);
                    moveDirection = Vector2.zero;
                    break;
            }
        }
    }

    private void SetPlayerDir()
    {
        dot.transform.position = touchPosition;

        dot.transform.position = new Vector2(
            Mathf.Clamp(dot.transform.position.x,
            circle.transform.position.x - 0.7f,
            circle.transform.position.x + 0.7f),
            Mathf.Clamp(dot.transform.position.y,
            circle.transform.position.y - 0.7f,
            circle.transform.position.y + 0.7f));

        moveDirection = (dot.transform.position - circle.transform.position).normalized;        
    }

    public Vector2 GetPlayerDir()
    {
        return moveDirection;
    }
}
