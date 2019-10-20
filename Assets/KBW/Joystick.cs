using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;
    
    void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        
    }

    // 터치패드를 누르고 있을 때
    public virtual void OnDrag(PointerEventData ped)
    {
        Debug.Log("Joystick: OnDrag()");

        Vector2 pos;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, pos.y * 2 + 1, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 조이스틱 이미지 이동
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3)
                , inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));

        }
    }

    // 터치하고 있을 때
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    // 터치를 중지할 때
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    // PlayerController 스크립트에서 inputVector.x 값을 받기 위해 사용될 함수
    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    // PlayerController 스크립트에서 inputVector.y 값을 받기 위해 사용될 함수
    public float GetVerticalValue()
    {
        return inputVector.y;
    }
}
