﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumSpace;

public class Range_Mummy : Enemy
{    
    private Coroutine findNearPlayer;   // 추적 코루틴 변수
    private Coroutine myUpdate;         // Update 코루틴 변수

    private Player player;           // 추적할 플레이어 리스트
    private Player target;              // 가장 가까운 플레이어        
    private Transform missileTarget;    // 미사일 발사 타겟 좌표
    public Transform detectPoint;       // 공격 감지 피봇
    public Transform missilePoint;      // 미사일 생성 위치
    public Vector2 detectRange;         // 공격 감지 범위
            
    private Enemy_Missile missile;

    private Animator animator;
    private Rigidbody2D rb;    

    protected override void OnEnable()
    {
        base.OnEnable(); // InitObject()        
    }

    // 능력치, 상태 값 설정
    public override void InitObject(int stageLevel)
    {
        startingHP = 100f + ((100f * 0.1f) * stageLevel);
        HP = startingHP;
        damage = 10f + ((10f * 0.1f) * stageLevel);
        moveSpeed = 30f;
        attackSpeed = 5f;
        dead = false;
        enemyState = EnemyState.IDLE;   // 적 상태
        enemyType = EnemyType.MONSTER;
        dir = 1;                        // 오른쪽 방향 할당
    }

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindObjectOfType<Player>();           // 플레이어 리스트 담기        

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();                
    }

    void Start()
    {
        findNearPlayer = StartCoroutine(FindNearPlayer(10f)); // 추적 코루틴 변수에 할당, 10초 마다 실행
        myUpdate = StartCoroutine(MyUpdate());
        
        onDeath += SetOnDeath;  // 죽었을 때 이벤트 추가
    }

    IEnumerator MyUpdate()
    {
        while (!dead)
        {
            switch (enemyState)
            {
                case EnemyState.IDLE:
                case EnemyState.TRACE:
                    DetectPlayer();  // 감지
                    TracePlayer();   // 감지한 플레이어한테 이동
                    break;
                case EnemyState.GAUGING:
                    Gauging();
                    break;
                case EnemyState.ATTACK:
                    Attack();
                    break;
                case EnemyState.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Attack()
    {
        // 공격 모션 -> 게이지 채우기 -> 다 채웠으면 공격

        // 공격 모션               
        animator.SetInteger("State", (int)enemyState);
        rb.velocity = new Vector2(0f, 0f);
    }


    // 공격 감지 기즈모
    public void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(detectPoint.position, detectRange);
    }

    // n초 마다 가장 가까운 플레이어를 찾기
    IEnumerator FindNearPlayer(float n)
    {
        while (true)
        {
            // 플레이어가 죽었다면
            if (player.dead)
            {
                target = null;

                // 코루틴 정지 후 while문 종료
                StopCoroutine(findNearPlayer);
                break;
            }

            target = player;

            // n초 마다 실행
            yield return new WaitForSeconds(n);
        }
    }

    void DetectPlayer()
    {
        // 공격 감지 범위 구현
        Collider2D[] hits = Physics2D.OverlapBoxAll(detectPoint.position, detectRange, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit)
            {
                // 플레이어를 감지 했다면
                if (hit.tag == "Player")
                {
                    missileTarget = hit.gameObject.transform;
                    enemyState = EnemyState.ATTACK;
                    return;                    
                }
            }
            else
            {
                Debug.Log("플레이어 미발견");
            }
        }
    }

    void Gauging()
    {
        if (missile.GetGauge() >= 1f)
        {
            Vector2 missileDir = (missileTarget.transform.position + new Vector3(0f, 0.5f, 0f) - missilePoint.position).normalized;
            float missileSpeed = 100f;
            missile.Fire(missileSpeed, missileDir);

            animator.speed = 1f;
            enemyState = EnemyState.ATTACK;
            return;
        }
        missile.SetGauge(0.1f);        
    }

    // Attack 애니메이션 이벤트 함수 -> 공격 게이지 시작
    public void StartAttackGauge()
    {
        animator.speed = 0f;
        missile = Enemy_Missile.Create(missilePoint.position, damage);
        enemyState = EnemyState.GAUGING;
    }

    // Attack 애니메이션 이벤트 함수 -> 공격 끝 -> 범위 내 플레이어에게 데미지 적용
    public void EndAttack()
    {
        enemyState = EnemyState.IDLE;
        animator.SetInteger("State", (int)enemyState);
    }

    // 이동 메서드
    void TracePlayer()
    {
        if (target != null)
        {
            if (enemyState == EnemyState.ATTACK)
            {
                return;
            }

            // 상태 설정
            enemyState = EnemyState.TRACE;
            // 애니메이터 파라미터 할당
            animator.SetInteger("State", (int)enemyState);

            // 방향 구하기
            Vector2 moveDir = (target.transform.position - pivot.position).normalized;

            // 바라보는 방향 설정
            if (moveDir.x > 0f)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                dir = 1;
            }
            else
            {
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                dir = -1;
            }

            // 이동            
            rb.velocity = moveDir * moveSpeed * Time.deltaTime;
        }
    }
    
    void SetOnDeath()
    {
        animator.speed = 0f;
        StartCoroutine(SetDissolve());
        Destroy(gameObject, 1f);
    }   
}