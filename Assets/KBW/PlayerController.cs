using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum PlayerState { idle, run};

    public Joystick joystick; 
    public float moveSpeed;     // 플레이어 이동속도

    private Vector3 moveVector; // 플레이어 이동벡터
    private PlayerState playerState; // 플레이어 상태
    private Animator animator;  // 플레이어 애니메이터

    
    void Start()
    {
        playerState = PlayerState.idle;
        moveVector = Vector3.zero;  // 플레이어 이동벡터 초기화

        animator = GetComponent<Animator>();
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
        Debug.Log(moveVector.x);
    }

    // 플레이어 이동    
    private void PlayerMove()
    {
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
    }

    // 플레이어 애니메이션
    private void PlayerAnimation()
    {
        if(moveVector.x >= 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
