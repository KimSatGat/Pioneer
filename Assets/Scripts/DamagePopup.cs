using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(bool isCritical)
    {
        textMesh.SetText("HIT!");
        if(!isCritical)
        {
            textMesh.fontSize = 5;
            textColor = new Color(1f, 197f / 255f, 0f);
        }
        else
        {
            textMesh.fontSize = 7;
            textColor = new Color(1f, 55f / 255f, 0f);
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

    // Create DamagePopup
    public static DamagePopup Create(Vector3 position, bool isCritical)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.instance.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(isCritical);

        return damagePopup;
        
    }
}
