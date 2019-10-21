using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick; 
    public float moveSpeed;     // 플레이어 이동속도

    private Vector3 moveVector; // 플레이어 이동벡터    
    
    void Start()
    {
        moveVector = Vector3.zero;  // 플레이어 이동벡터 초기화
    }

    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    // 조이스틱 입력값 받아오기
    public void HandleInput()
    {
        Vector2 moveDir = joystick.GetPlayerDir();

        moveVector = moveDir;
    }

    // 플레이어 이동    
    public void Move()
    {
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
    }
}
