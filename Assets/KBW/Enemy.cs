using System.Collections;
using UnityEngine;


// 추적, 이동 기능 제공
public class Enemy : LivingObject
{
    enum EnemyState {IDLE, TRACE, ATTACK, DIE};

    private Coroutine findNearPlayer;   // 추적 코루틴 변수
    private Player[] players;           // 추적할 플레이어 리스트
    private Player target;              // 가장 가까운 플레이어    

    protected override void OnEnable()
    {
        base.OnEnable();    // 스텟 초기화
        players = GameObject.FindObjectsOfType<Player>();  // 플레이어 리스트 담기
        findNearPlayer = StartCoroutine(FindNearPlayer()); // 추적 코루틴 변수에 할당
    }
    
    // 10초 마다 가장 가까운 플레이어를 찾기
    private IEnumerator FindNearPlayer()
    {
        while(true)
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
            else if(!players[0].dead && players[1].dead)
            {
                target = players[0];
            }

            // P1가 죽고 P2가 살았다면
            else if(players[0].dead && !players[1].dead)
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
            yield return new WaitForSeconds(10f);
        }
    }

    // 이동 메서드
    protected void TracePlayer()
    {
        
        if(target != null)
        {
            // 방향 구하기
            Vector2 moveDir = (target.transform.position - transform.position).normalized;

            // 이동
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
    }
}
