using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumSpace;

public class Melee_Skeleton : Enemy
{
    private Coroutine findNearPlayer;   // 추적 코루틴 변수
    private Coroutine myUpdate;         // Update 코루틴 변수

    private Player player;           // 추적할 플레이어 리스트
    private Player target;              // 가장 가까운 플레이어    
    
    public Transform attackPivot;      // 공격 레이가 나가는 피봇
    public GameObject attackUI;         // 공격 UI
    public Image attackGauge;           // 공격게이지 UI
    public Transform detectPoint;       // 공격 감지 피봇
    public Vector2 detectRange;         // 공격 감지 범위
    public float[] lens = { 1.26f, 1.31f, 1.32f, 1.31f, 1.24f };   // 공격 감지 레이캐스트 마다 길이 할당 -> 기즈모로 직접 길이 구함..
    List<Vector2> dirs = new List<Vector2>();   // 공격 감지 방향 리스트

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
        dir = 1;                        // 오른쪽 방향 할당
    }

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindObjectOfType<Player>();  // 플레이어 리스트 담기        

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();        

        // 공격 감지 레이캐스트 방향 할당
        for (int i = 0; i < lens.Length; i++)
        {
            dirs.Add(new Vector2(
                Mathf.Cos((-20 + 10f * i) * Mathf.Deg2Rad),
                Mathf.Sin((-20 + 10f * i) * Mathf.Deg2Rad)
                ));
        }
    }

    void Start()
    {
        findNearPlayer = StartCoroutine(FindNearPlayer(10f)); // 추적 코루틴 변수에 할당, 10초 마다 실행
        myUpdate = StartCoroutine(MyUpdate());

        onDeath += OffAttackUI;     // 죽었을 때 이벤트 추가
        onDeath += SetOnDeath;      
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
                    // y값 계산
                    float offsetPosY = Mathf.Abs(hit.gameObject.transform.position.y - pivot.position.y);

                    if(offsetPosY <= 0.5f)
                    {
                        enemyState = EnemyState.ATTACK;
                        return;
                    }
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
        if (attackGauge.fillAmount >= 1f)
        {
            attackGauge.fillAmount = 0f;
            animator.speed = 1f;
            enemyState = EnemyState.ATTACK;
            return;
        }

        attackGauge.fillAmount += attackSpeed / 100f;
    }

    // Attack 애니메이션 이벤트 함수 -> 공격 게이지 시작
    public void StartAttackGauge()
    {
        animator.speed = 0f;
        enemyState = EnemyState.GAUGING;
    }

    // Attack 애니메이션 이벤트 함수 -> 공격 끝 -> 범위 내 플레이어에게 데미지 적용
    public void EndAttack()
    {
        // 레이캐스트 기즈모 표시

        for (int i = 0; i < 5; i++)
        {
            Debug.DrawRay(attackPivot.position, new Vector3(dir, 1f, 0) * dirs[i] * lens[i], Color.yellow);
        }

        // 플레이어에게 데미지를 주었는지 판단
        bool isDamaed = false;
        // 레이캐스트 
        for (int i = 0; i < 5; i++)
        {
            // 데미지를 주었다면 break
            if (isDamaed)
            {
                break;
            }
            RaycastHit2D[] hits = Physics2D.RaycastAll(attackPivot.position, new Vector3(dir, 1f, 0) * dirs[i], lens[i]);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Player")
                {
                    // 플레이어 데미지 주기

                    // y값 계산
                    float offsetPosY = Mathf.Abs(hit.transform.position.y - pivot.position.y);

                    if(offsetPosY <= 0.5f)
                    {
                        // 데미지는 한번만 주기 때문에 true
                        isDamaed = true;

                        Player player = hit.collider.gameObject.GetComponent<Player>();
                        player.OnDamage(damage);
                        // foreach문 빠져나가기
                        break;
                    }

                }
            }
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

    public void OnAttackUI()
    {
        attackUI.SetActive(true);
    }
    public void OffAttackUI()
    {
        attackUI.SetActive(false);
    }
}