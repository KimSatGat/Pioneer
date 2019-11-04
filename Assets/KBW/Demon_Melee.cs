using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Melee : Enemy, IMeleeEnemy
{
    public Transform attackRange;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    // 능력치, 상태 값 설정
    public override void InitObject()
    {
        startingHP = 100f;
        HP = startingHP;
        damage = 10f;
        speed = 3f;
        dead = false;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    void IMeleeEnemy.Attack()
    {
        // 공격 모션 -> 게이지 채우기 -> 다 채웠으면 공격
    }

    void IMeleeEnemy.DetectPlayer()
    {
        // 공격 감지 범위 구현

    }    
}
