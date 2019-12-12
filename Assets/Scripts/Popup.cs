using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumSpace;

public class Popup : MonoBehaviour
{
    public AudioClip hitClip;
    public AudioClip criticalHitClip;
    private AudioSource audioSource;

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;    

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(PopupType popupType)
    {
        switch(popupType)
        {
            case PopupType.HIT:
                textMesh.SetText("HIT!");
                textMesh.fontSize = 5;
                textColor = new Color(1f, 197f / 255f, 0f);
                audioSource.clip = hitClip;
                break;
            case PopupType.CRITICAL:
                textMesh.SetText("CRITICAL!");
                textMesh.fontSize = 7;
                textColor = new Color(1f, 55f / 255f, 0f);
                audioSource.clip = criticalHitClip;
                break;
            case PopupType.MISS:
                textMesh.SetText("MISS~");
                textMesh.fontSize = 5;
                textColor = new Color(233f / 255f, 122f / 255f, 216f / 255f);
                break;
        }
        textMesh.color = textColor;        
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(0.7f, 1f) * 2f;
    }

    private void Update()
    {        
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 2f * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            // First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }


        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public static Popup CreatePopup(Vector3 position, PopupType popupType)
    {
        Transform hitPopupTransform = Instantiate(GameAssets.instance.pfPopup, position, Quaternion.identity);
        Popup hitPopup = hitPopupTransform.GetComponent<Popup>();
        hitPopup.Setup(popupType);
        hitPopup.audioSource.Play();
        return hitPopup;        
    }        
}
