using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    int idx;
    Image image;
    public RectTransform abilityPanel;
    
    private void OnEnable()
    {
        image = GetComponent<Image>();

        // 랜덤으로 인덱스를 정한다
        idx = Random.Range(0, 3);

        // 인덱스에 맞는 이미지로 바꾼다
        image.sprite = GameAssets.instance.abilitySprite[idx];
    }

    // 어빌리티 구현
    public void SelectAbility()
    {
        // 테스트용 밀리플레이어 공격 +10
        if(idx == 0)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.damage += 10f;
        }

        // 테스트용 밀리플레이어 공격속도 +0.5
        else if(idx == 1)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.attackSpeed += 0.5f;
        }

        // 테스트용 밀리플레이어 최대 체력 +50
        else if(idx == 2)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.HP += 50;
        }

        // 패널 닫기
        abilityPanel.gameObject.SetActive(false);
    }       
}
