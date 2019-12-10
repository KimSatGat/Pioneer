using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;

    public Image barImage;
    public Image damagedBarImage;
    private Color damagedColor;
    private float damagedHealthFadeTimer;
    public HealthSystem healthSystem;
    private LivingObject livingObject;

    private void Awake()
    {
        damagedColor = damagedBarImage.color;
        damagedColor.a = 0f;
        damagedBarImage.color = damagedColor;
        livingObject = transform.root.GetComponent<LivingObject>(); // 최상위 오브젝트의 LivingObject 가져오기
    }

    private void Start()
    {
        healthSystem = new HealthSystem((int)livingObject.HP);      // 오브젝트 HP 값 할당        
        SetHealth(healthSystem.GetHealthNormalized());              // 정규화해서 Bar에 할당
        healthSystem.OnDamaed += HealthSystem_OnDamaged;            // HealthSystem_OnDamaged 메서드 추가
    }

    private void Update()
    {        
        if (damagedColor.a > 0)
        {
            damagedHealthFadeTimer -= Time.deltaTime;
            if(damagedHealthFadeTimer < 0)
            {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = damagedColor;
            }
        }
    }

    public void HealthStstem_OnHealed()
    {
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void HealthSystem_OnDamaged()
    {        
        if(damagedColor.a <= 0)
        {
            // Damaged bar image is invisible
            damagedBarImage.fillAmount = barImage.fillAmount;            
        }

        damagedColor.a = 1;
        damagedBarImage.color = damagedColor;
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;

        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float healthNormalized)
    {
        barImage.fillAmount = healthNormalized;
    }
}
