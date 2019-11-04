using UnityEngine;

// 근접 공격 Enemy를 위한 인터페이스
// 플레이어 감지, 공격 가상 함수 제공
public interface IMeleeEnemy
{
    void DetectPlayer();
    void Attack();
}
