﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;

public class Enemy : LivingObject
{    
    protected EnemyState enemyState;      // 적 상태
    protected EnemyType enemyType;

    public Transform pivot;

    private HealthBarFade healthBarFade;    // 체력바
    private Material material;
    private Color materialTintColor;    // 틴트 효과를 위한 색상값
    private float dissolveAmout = 0f;   // Dissolve 효과 값
    private string dissolveShader = "Shader Graphs/Dissolve";
    private string ghostShader = "Shader Graphs/Ghost";

    protected virtual void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        materialTintColor = new Color(1f, 0f, 0f, 150f / 255f);

        healthBarFade = GetComponentInChildren<HealthBarFade>();
    }
    
    // 데미지를 입는 기능
    public void OnDamage(float damage, PlayerType hitPlayerType, bool isCritical)
    {
        switch(enemyType)
        {
            case EnemyType.MONSTER:

                // 데미지만큼 체력 감소
                HP -= damage;

                // 틴트 효과
                StartCoroutine(SetTint());

                // HIT 효과 카메라 흔들림
                if (isCritical)
                {
                    Popup.CreatePopup(transform.position, PopupType.CRITICAL);                    
                    CameraShake.instance.SetCameraSake(3f, 3f, 0.15f);
                }
                else
                {
                    Popup.CreatePopup(transform.position, PopupType.HIT);
                    CameraShake.instance.SetCameraSake(1f, 1f, 0.15f);                    
                }

                // HP UI 감소 효과
                healthBarFade.healthSystem.Damage((int)damage);

                // Combo 효과
                Combo.instance.ComboPlus();
                Combo.instance.SetCombo();
                
                // 체력이 0 이하이고 죽지않았다면
                if (HP <= 0 && !dead)
                {
                    Die();
                }
                break;         
        }                                                                
    }

    public Vector3 GetPivot()
    {
        return pivot.position;
    }

    // 틴트 효과
    IEnumerator SetTint()
    {
        while (true)
        {
            if (materialTintColor.a > 0)
            {
                materialTintColor.a = Mathf.Clamp01(materialTintColor.a - 6f * Time.deltaTime);

                material.SetColor("_Tint", materialTintColor);
            }
            else
            {
                materialTintColor = new Color(1f, 0f, 0f, 150f / 255f);
                StopCoroutine(SetTint());
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Dissolve 효과
    protected IEnumerator SetDissolve()
    {
        // 셰이더 변경
        material.shader = Shader.Find(dissolveShader);
        float dissolveSpeed = 5f;

        if(enemyType == EnemyType.GHOST_BLUE)
        {
            material.SetColor("_DissolveColor", new Color(19f / 255f, 19f / 255f, 214f / 255f));
        }

        while (true)
        {
            if (dissolveAmout < 1f)
            {
                dissolveAmout = Mathf.Clamp01(dissolveAmout + dissolveSpeed * Time.deltaTime);
                material.SetFloat("_DissolveAmount", dissolveAmout);
            }

            else
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 유령 효과
    void SetGhost()
    {
        // 셰이더 변경
        material.shader = Shader.Find(ghostShader);

        if(enemyType == EnemyType.GHOST_BLUE)
        {
            material.SetColor("_Color", new Color(28f / 255f, 42f / 255f, 191f / 255f));
        }
        else
        {
            material.SetColor("_Color", new Color(191f / 255f, 42f / 255f, 28f / 255f));
        }
    }
}


/*
 2인용 모드 때 쓰일 코드
 case EnemyType.GHOST_RED:
                if(hitPlayerType == PlayerType.RANGE)
                {
                    Die();
                }
                else
                {
                    // MISS 효과
                    Popup.CreatePopup(transform.position, PopupType.MISS);
                }
                break;
            case EnemyType.GHOST_BLUE:
                if (hitPlayerType == PlayerType.MELEE)
                {
                    Die();
                }
                else
                {
                    // MISS 효과
                    Popup.CreatePopup(transform.position, PopupType.MISS);
                }
                break;
 */
