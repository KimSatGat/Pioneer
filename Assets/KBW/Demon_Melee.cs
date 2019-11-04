using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Melee : LivingObject
{
    enum EnemyState { IDLE, TRACE, ATTACK, DIE, GAUGING };

    private Coroutine findNearPlayer;   // 추적 코루틴 변수    
    private Coroutine myUpdate;         // Update 코루틴 변수

    private Player[] players;           // 추적할 플레이어 리스트
    private Player target;              // 가장 가까운 플레이어
    private EnemyState enemyState;      // 적 상태
    
    public RectTransform attackGauge;   // 공격게이지 UI
    public Transform attackPoint;       // 공격 지점
    public Vector2 attackRange;         // 공격 범위

    private float attackGaugeValue;     // 공격UI 게이지 값
    private float maxGauge;             // 공격UI 최대 게이지 값

    private Animator animator;
    private Rigidbody2D rigidbody2D;

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
        speed = 30f;
        dead = false;
        attackGaugeValue = 0f;  // 현재 공격 게이지
        maxGauge = 1f;       // 도달해야 할 공격 게이지
        enemyState = EnemyState.IDLE;   // 적 상태
    }

    void Awake()
    {
        players = GameObject.FindObjectsOfType<Player>();  // 플레이어 리스트 담기

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        findNearPlayer = StartCoroutine(FindNearPlayer(10f)); // 추적 코루틴 변수에 할당, 10초 마다 실행
        myUpdate = StartCoroutine(MyUpdate());
    }
        

    IEnumerator MyUpdate()
    {
        while(!dead)
        {
            switch(enemyState)
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
        rigidbody2D.velocity = new Vector2(0f, 0f);
    }


    // 공격 범위 기즈모
    public void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(attackPoint.position, attackRange);
    }
    
    // n초 마다 가장 가까운 플레이어를 찾기
    IEnumerator FindNearPlayer(float n)
    {
        while (true)
        {
            // 둘다 죽었다면 찾기 중지
            if (players[0].dead && players[1].dead)
            {
                target = null;

                // 코루틴 정지 후 while문 종료
                StopCoroutine(findNearPlayer);
                break;
            }

            // P1가 살고 P2가 죽었다면
            else if (!players[0].dead && players[1].dead)
            {
                target = players[0];
            }

            // P1가 죽고 P2가 살았다면
            else if (players[0].dead && !players[1].dead)
            {
                target = players[1];
            }

            // 둘 다 살았다면
            else
            {
                // 거리 측정        
                float EnemytoP1 = Vector2.Distance(players[0].transform.position, transform.position);
                float EnemytoP2 = Vector2.Distance(players[1].transform.position, transform.position);

                // P1이 더 가깝다면
                if (EnemytoP1 < EnemytoP2)
                {
                    // P1 할당
                    target = players[0];
                }

                // P2가 더 가깝다면
                else
                {
                    // P2 할당
                    target = players[1];
                }
            }

            // 10초 마다 실행
            yield return new WaitForSeconds(n);
        }
    }

    void DetectPlayer()
    {        
        // 공격 감지 범위 구현
        Collider2D hit = Physics2D.OverlapBox(attackPoint.position, attackRange, 0f);

        if (hit)
        {
            // 플레이어를 감지 했다면
            if (hit.tag == "Player")
            {
                enemyState = EnemyState.ATTACK;                
            }
        }
        else
        {
            Debug.Log("플레이어 미발견");
        }                    
    }

    void Gauging()
    {        
        if(attackGaugeValue >= maxGauge)
        {            
            attackGauge.sizeDelta = new Vector2(0f, 1.8f);
            attackGaugeValue = 0f;
            animator.speed = 1f;
            enemyState = EnemyState.ATTACK;
            return;
        }

        attackGauge.sizeDelta = attackGauge.sizeDelta + new Vector2(0.1f, 0f);
        attackGaugeValue += 0.1f;                                
    }

    public void StartAttackGauge()
    {
        animator.speed = 0f;
        enemyState = EnemyState.GAUGING;
    }

    public void EndAttack() 
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                // 플레이어를 감지 했다면
                if (hit.tag == "Player")
                {
                    // 플레이어 공격
                    Player player = hit.GetComponent<Player>();
                    player.OnDamage(damage);
                }
            }
        }
        else
        {
            Debug.Log("플레이어 미발견");
        }

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
            Vector2 moveDir = (target.transform.position - transform.position).normalized;

            // 바라보는 방향 설정
            if(moveDir.x > 0f)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            else
            {
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }

            // 이동            
            rigidbody2D.velocity = moveDir * speed * Time.deltaTime;
        }
    }
}
