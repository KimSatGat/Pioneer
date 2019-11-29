using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator.GetComponent<Animator>();
        animator.gameObject.SetActive(false);
    }
    
    void Update()
    {
        // wave3까지 다 잡았다면
        if(GameManager.instance.wave3_enemies.childCount <= 0)
        {
            // 애니메이션 활성화
            animator.gameObject.SetActive(true);           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(GameManager.instance.wave3_enemies.childCount <= 0)
            {
                GameManager.instance.NextStage();
            }
        }
    }
}
