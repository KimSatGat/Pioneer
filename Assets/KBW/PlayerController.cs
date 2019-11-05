using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingObject
{
    enum PlayerState { IDLE, RUN, ATTACK, DIE};

    public Joystick joystick;       // 조이스틱
    public float moveSpeed;         // 플레이어 이동속도
    public float attackSpeed;       // 플레이어 공격속도
    public Transform attackPoint;   // 공격 지점
    public Vector2 attackRange;     // 공격 범위

    private PlayerState playerState;    // 플레이어 상태
    private Vector2 playerDir;  // 플레이어 방향
    
    private Vector3 moveVector; // 플레이어 이동벡터
    private Animator animator;  // 플레이어 애니메이터    
    private Rigidbody2D rigidbody2D;


    private void Awake()
    {
        playerState = PlayerState.IDLE;     // 플레이어 상태 초기화
        playerDir = Vector2.right;
        moveVector = Vector3.zero;          // 플레이어 이동벡터 초기화
        attackSpeed = 1f;
        
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {        
        StartCoroutine("Attack");
    }

    void Update()
    {                
        HandleInput();
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
        //transform.Translate(moveVector * moveSpeed * Time.deltaTime);  기존에 썻던 좌표값 변화를 통한 이동방식 -> 회전 했을때 반대 방향으로 가서 아래 방법을 씀
        rigidbody2D.velocity = new Vector2(moveVector.x, moveVector.y);
    }

    // 플레이어 애니메이션
    private void PlayerAnimation()
    {
        // 오른쪽 이동
        if(moveVector.x > 0)
        {
            playerState = PlayerState.RUN;
            playerDir = Vector2.right;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        }
        // 왼쪽 이동
        else if(moveVector.x < 0)
        {
            playerState = PlayerState.RUN;
            playerDir = Vector2.left;
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if(playerState == PlayerState.ATTACK)
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
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(attackPoint.position, attackRange);
    }

    // 공격
    IEnumerator Attack()
    {
        while(true)
        {
            Collider2D hit = Physics2D.OverlapBox(attackPoint.position, attackRange, 0f);
            
            if(hit)
            {
                if(hit.tag == "Enemy" && moveVector.x == 0f)
                {
                    playerState = PlayerState.ATTACK;
                    animator.SetInteger("State", (int)playerState);
                    Debug.Log("적 감지");
                }

            }
            else
            {
                Debug.Log("적 미발견");
            }

            yield return new WaitForSeconds(attackSpeed);
        }
    }

    // 공격 모션이 끝 -> 적이 있다면 데미지 적용, 원래 상태로 복귀
    public void EndAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f);
        if(collider2Ds.Length > 0)
        {
            foreach (Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Enemy")
                {
                    LivingObject livingObject = collider.GetComponent<LivingObject>();
                    livingObject.OnDamage(damage);
                    Debug.Log("적 공격!");
                }
            }
        }
    
        // 원래 상태로 복귀
        playerState = PlayerState.IDLE;
        animator.SetInteger("State", (int)playerState);
    }
}
