using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;


public class Melee_Player : Player
{
    public Joystick joystick;           // 조이스틱
    public Transform attackPoint;       // 공격 감지 피봇    
    public Transform dashPoint;         // 대시 감지 피봇    
    public Vector2 attackRange;         // 공격 범위
    public Vector2 dashRange;           // 대시 범위

    private PlayerState playerState;    // 플레이어 상태     

    private Vector3 moveVector;         // 플레이어 이동벡터
    private Animator animator;          // 플레이어 애니메이터    
    private Rigidbody2D rb;

    private HealthBarFade healthBarFade; // 체력바

    private int criticalChance;         // 치명타 확률

   // private AudioSource player; // 플레이어 오디오 소스
   // public AudioClip player_hited; // 플레이어 맞을 때 오디오 클립

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void InitObject(int stageLevel)
    {
        startingHP = 100f;
        HP = startingHP;
        damage = 30f;
        moveSpeed = 5f;
        attackSpeed = 1f;
        dead = false;
        playerState = PlayerState.IDLE;     // 플레이어 상태 초기화
        playerType = PlayerType.MELEE;      // 플레이어 타입
        dir = 1;                            // 오른쪽 방향 할당
        moveVector = Vector3.zero;          // 플레이어 이동벡터 초기화
        criticalChance = 15;                // 치명타 확률 15%
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        healthBarFade = GetComponentInChildren<HealthBarFade>();
    }

    void Start()
    {
        StartCoroutine("Attack");

        //player = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();
        Attack();
    }

    private void FixedUpdate()
    {
        PlayerAnimation();
        PlayerMove();
    }

    // 조이스틱 입력값 받아오기
    public void HandleInput()
    {
        Vector2 moveDir = joystick.GetPlayerDir();
        moveVector = moveDir;
    }

    // 플레이어 이동    
    private void PlayerMove()
    {
        rb.velocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed / 2);
    }

    // 플레이어 애니메이션
    private void PlayerAnimation()
    {
        // 오른쪽 이동
        if (moveVector.x > 0)
        {
            playerState = PlayerState.RUN;
            dir = 1;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        }
        // 왼쪽 이동
        else if (moveVector.x < 0)
        {
            playerState = PlayerState.RUN;
            dir = -1;
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (playerState == PlayerState.ATTACK)
        {
            return;
        }
        // 정지
        else
        {
            playerState = PlayerState.IDLE;
        }

        animator.SetInteger("State", (int)playerState);
    }

    // 공격 범위 기즈모
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(3f, 3f, 0f, 150 / 255f);
        //Gizmos.DrawCube(dashPoint.position, dashRange);

        Gizmos.color = new Color(1f, 1f, 1f, 150 / 255f);
        Gizmos.DrawCube(attackPoint.position, attackRange);
    }

    // 공격
    IEnumerator Attack()
    {
        while (!dead)
        {
            Collider2D[] attackHits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f);
            bool isAttack = false;

            if (playerState != PlayerState.ATTACK)
            {

                // 공격 범위에 적이 있다면
                if (attackHits.Length > 0)
                {
                    foreach (Collider2D attackHit in attackHits)
                    {
                        if (attackHit.tag == "Enemy" && moveVector.x == 0f && moveVector.y == 0f)
                        {
                            playerState = PlayerState.ATTACK;
                            animator.SetInteger("State", (int)playerState);
                            isAttack = true;
                        }
                    }
                }

                if (!isAttack)
                {
                    // Dash 체크
                    Collider2D[] dashHits = Physics2D.OverlapBoxAll(dashPoint.position, dashRange, 0f);

                    if (dashHits.Length > 0)
                    {
                        foreach (Collider2D dashHit in dashHits)
                        {
                            if (dashHit.tag == "Enemy" && moveVector.x == 0f && moveVector.y == 0f)
                            {
                                Enemy enemy = dashHit.GetComponent<Enemy>();
                                Vector3 hitPos = enemy.GetPivot();

                                // 대쉬
                                float distance = Vector3.Distance(hitPos, transform.position);
                                float offset = 1f;
                                Vector3 dashDir = (hitPos - transform.position).normalized;
                                Vector3 beforeDashPos = transform.position;

                                transform.position += dashDir * (distance - offset);

                                Transform dashEffect = Instantiate(GameAssets.instance.pfDashEffect, beforeDashPos, Quaternion.identity);
                                float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
                                dashEffect.eulerAngles = new Vector3(0f, 0f, angle);
                                Debug.Log(distance);
                                dashEffect.localScale = new Vector3(distance / 4f, 1f, 1f);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 모션이 끝 -> 적이 있다면 데미지 적용, 원래 상태로 복귀
    public void EndAttack()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f);
        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.tag == "Enemy")
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    float hitPosY = enemy.GetPivot().y;

                    float offsetPosY = Mathf.Abs(hitPosY - transform.position.y);
                    if (offsetPosY <= 0.5f)
                    {
                        bool isCritical;
                        if (Random.Range(1, 101) < criticalChance)
                        {
                            isCritical = true;
                        }
                        else
                        {
                            isCritical = false;
                        }
                        enemy.OnDamage(damage, PlayerType.MELEE, isCritical);
                    }
                }
            }
        }
        // 원래 상태로 복귀
        playerState = PlayerState.IDLE;
        animator.SetInteger("State", (int)playerState);
    }

    public override void OnDamage(float damage)
    {
        //맞을 때 소리 재생
       // player.PlayOneShot(player_hited, 1f);
        // 체력 감소
        base.OnDamage(damage);

        // HP UI 감소 효과
        healthBarFade.healthSystem.Damage((int)damage);
    }
}
