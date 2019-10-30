using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum PlayerState { IDLE, RUN, ATTACK};

    public Joystick joystick;   // 조이스틱
    public float moveSpeed;     // 플레이어 이동속도
    public float attackSpeed;   // 플레이어 공격속도
    public Transform attackRange;

    private PlayerState playerState; // 플레이어 상태
    private Vector3 moveVector; // 플레이어 이동벡터
    private Animator animator;  // 플레이어 애니메이터
                
    void Start()
    {        
        playerState = PlayerState.IDLE;  // 플레이어 상태 초기화
        moveVector = Vector3.zero;       // 플레이어 이동벡터 초기화
        attackSpeed = 1f;

        animator = GetComponent<Animator>();

        StartCoroutine("PlayerEnemyDetect");
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
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
    }

    // 플레이어 애니메이션
    private void PlayerAnimation()
    {
        // 오른쪽 이동
        if(moveVector.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            playerState = PlayerState.RUN;
        }
        // 왼쪽 이동
        else if(moveVector.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            playerState = PlayerState.RUN;
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
        Gizmos.DrawCube(attackRange.position, new Vector2(2f, 2f));
    }

    // 공격 범위에 적이 있는지 감지
    IEnumerator PlayerEnemyDetect()
    {
        while(true)
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                attackRange.position,                       // 시작 위치
                new Vector2(2f, 2f),                        // 상자 크기
                0f,                                         // 회전 각도
                new Vector2(transform.position.x, 0f),      // 방향
                2.5f                                        // 최대 거리
                );

            if(hit.collider.tag == "Enemy" && moveVector.x == 0)
            {
                playerState = PlayerState.ATTACK;
                animator.SetInteger("State", (int)playerState);
            }
        
            Debug.Log(hit.collider.tag);
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    // 공격 모션이 끝 -> 적이 있다면 데미지 적용, 원래 상태로 복귀
    public void EndAttack()
    {
        // 적이 있는지 확인
        RaycastHit2D hit = Physics2D.BoxCast(
                attackRange.position,                       // 시작 위치
                new Vector2(2f, 2f),                        // 상자 크기
                0f,                                         // 회전 각도
                new Vector2(transform.position.x, 0f),      // 방향
                2.5f                                        // 최대 거리
                );

        // 공격 범위에 적이 있다면
        if (hit.collider.tag == "Enemy")
        {
            Debug.Log("적 공격!");
        }
    
        // 원래 상태로 복귀
        playerState = PlayerState.IDLE;
        animator.SetInteger("State", (int)playerState);
    }

}
