using UnityEngine;

// 데미지를 입는 객체들이 공통적으로 가져야 하는 인터페이스
public interface IDamageable
{
    // 데미지를 입을 수 있는 타입들은 IDamageable을 상속하고
    // OnDamage 메서드를 반드시 구현 해야함
    
    void OnDamage(float damage); // 받은 피해(damage)
}