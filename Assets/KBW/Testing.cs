using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public float[] lens = { 1.53f, 1.48f, 1.39f, 1.28f, 1.155f };

    void Update()
    {
        // 공격 감지 방향 리스트
        List<Vector2> dirs = new List<Vector2>();

        // 방향 할당
        for(int i = 0; i < 5; i++)
        {
            dirs.Add(new Vector2(
                Mathf.Cos( (5 + 13.75f * i) * Mathf.Deg2Rad ),
                Mathf.Sin( (5 + 13.75f * i) * Mathf.Deg2Rad )
                ));
        }
                
        // 피봇 구하기
        Vector3 pivot = transform.position + new Vector3(0f, -0.5f, 0f);
       
        

        }    
    /*
    void EndAttack()
    {
        // 레이캐스트 기즈모 표시
        for (int i = 0; i < 5; i++)
        {
            Debug.DrawRay(pivot, dirs[i] * lens[i], Color.yellow);
        }

        // 레이캐스트 
        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(pivot, dirs[i], lens[i]);
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log(hit.collider.tag);
            }
        }
    }
    */
}
