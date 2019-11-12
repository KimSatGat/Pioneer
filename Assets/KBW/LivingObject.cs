using System;
using UnityEngine;

// 생명체 최상위 클래스
// 체력, 공격력, 이동 속도, 데미지, 사망 기능, 사망 이벤트 제공

public class LivingObject : MonoBehaviour, IDamageable
{
    public float startingHP = 100f; // 시작 체력
    public float HP;                // 현재 체력
    public float damage;            // 공격력 
    public float moveSpeed;         // 이동 속도
    public float attackSpeed;       // 공격 속도
    public int dir;                 // 바라보는 방향 오른쪽: 1 왼쪽: -1
    public bool dead;               // 사망 상태
    public event Action onDeath;    // 사망시 발동할 이벤트
    SpriteRenderer spriteRenderer;

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {        
        InitObject();
    }

    // 생명체 초기화 기능
    public virtual void InitObject() { }

    // 데미지를 입는 기능
    public virtual void OnDamage(float damage)
    {
        // 데미지만큼 체력 감소
        HP -= damage;

        // 체력이 0 이하이고 아직 죽지 않았다면 Die 메서드 실행
        if(HP <= 0 && !dead)
        {
            Die();
        }
    }

    // 체력 회복 기능
    public virtual void RestoreHP(float heal)
    {
        // 죽었으면 return
        if(dead)
        {
            return;
        }

        // 체력 추가
        HP += heal;
    }

    // 사망 처리
    public virtual void Die()
    {
        // onDead 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        // 사망 상태를 참으로 변경
        dead = true;
    }
}
