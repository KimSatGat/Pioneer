using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;

public class Player : LivingObject
{   
    public PlayerType playerType;

    // 데미지를 입는 기능
    public virtual void OnDamage(float damage)
    {
        // 데미지만큼 체력 감소
        HP -= damage;

        // 체력이 0 이하이고 아직 죽지 않았다면 Die 메서드 실행
        if (HP <= 0 && !dead)
        {
            Die();
        }
    }
}
