using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    int idx;
    Image image;
    public RectTransform abilityPanel;
    public Text abilityText;
    
    private void OnEnable()
    {            
        image = GetComponent<Image>();

        // 어빌리티 개수 만큼 랜덤으로 인덱스를 정한다
        idx = Random.Range(0, GameAssets.instance.abilitySprite.Count);

        // 인덱스에 맞는 이미지로 바꾼다
        image.sprite = GameAssets.instance.abilitySprite[idx];

        // 인덱스에 맞는 Text로 변경
        if(idx == 0)
        {
            abilityText.text = "공격력 증가";
        }
        else if(idx == 1)
        {
            abilityText.text = "공격속도 증가";
        }
        else if(idx == 2)
        {
            abilityText.text = "최대 체력 증가";
        }
        else if(idx == 3)
        {
            abilityText.text = "체력 회복";
        }
    }

    // 어빌리티 구현
    public void SelectAbility()
    {
        // 밀리플레이어 공격력 +10
        if(idx == 0)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.damage += 10f;            
        }

        // 밀리플레이어 공격속도 +0.5
        else if(idx == 1)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.attackSpeed += 0.5f;            
        }

        // 밀리플레이어 최대 체력 +50
        else if(idx == 2)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.startingHP += 50f;            
        }

        // 밀리 플레이어 체력 회복
        else if(idx == 3)
        {
            Melee_Player melee_Player = FindObjectOfType<Melee_Player>();
            melee_Player.RestoreHP(30f);            
        }

        // 패널 닫기
        abilityPanel.gameObject.SetActive(false);
    }       
}
