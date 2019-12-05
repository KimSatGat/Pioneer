using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    // 플레이어 관리
    public Transform player;
    public Transform playerPosition;

    // 몬스터 관리
    public Transform[] SpwanList;
    public Transform[] wave1_Pos;
    public Transform[] wave2_Pos;
    public Transform[] wave3_Pos;
    public Transform wave1_enemies;
    public Transform wave2_enemies;
    public Transform wave3_enemies;

    // 스테이지 관리
    public TextMeshProUGUI stageText;
    public int stageLevel = 1;
    public int waveMonsterCount = 1;
    public int maxMonsterCount = 6;    

    private void Awake()    
    {
        SettingResolution();

        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
        }
    }

    private void Start()
    {    
        // 웨이브마다 생성 위치값 할당
        for (int i = 0; i < instance.SpwanList[0].childCount; i++)
        {            
            wave1_Pos[i] = SpwanList[0].GetChild(i).transform;
            wave2_Pos[i] = SpwanList[1].GetChild(i).transform;
            wave3_Pos[i] = SpwanList[2].GetChild(i).transform;
        }

        MakeStage();
    }

    private void Update()
    {             
        /*
        if(instance.wave1_enemies.childCount <= 0)
        {
            instance.wave2_enemies.gameObject.SetActive(true);
        }

        if(instance.wave2_enemies.childCount <= 0)
        {
            instance.wave3_enemies.gameObject.SetActive(true);            
        }

        if(instance.wave3_enemies.childCount <= 0)
        {
            ClearStage();
            MakeStage();
            
        }
        */
    }

    // 해상도, 가로 모드 설정
    void SettingResolution()
    {
        // 해상도 설정
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);

        // 가로 모드 설정
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }

    // Wave3 까지 다 깼다면
    void ClearStage()
    {
        // 플레이어 원위치
        player.position = playerPosition.position;
        

        // 스테이지 레벨 상승
        instance.stageLevel++;
        // 5단위인지 체크해서 최대 적 수 증가
        if(instance.stageLevel % 5 == 0)
        {
            instance.waveMonsterCount++;
            if(instance.waveMonsterCount > instance.maxMonsterCount)
            {
                instance.waveMonsterCount = instance.maxMonsterCount;
            }
        }
    }

    // 다음 스테이지 생성
    void MakeStage()
    {
        // 몬스터 미리 생성해두기
        for (int i = 0; i < instance.waveMonsterCount; i++)
        {
            int idx;
            idx = Random.Range(0, 5);
            Instantiate(
                GameAssets.instance.enemyList[idx],
                GameManager.instance.wave1_Pos[i].position,
                Quaternion.identity).parent = instance.wave1_enemies;

            idx = Random.Range(0, 5);
            Instantiate(
                GameAssets.instance.enemyList[idx],
                GameManager.instance.wave2_Pos[i].position,
                Quaternion.identity).parent = instance.wave2_enemies;

            idx = Random.Range(0, 5);
            Instantiate(
                GameAssets.instance.enemyList[idx],
                GameManager.instance.wave3_Pos[i].position,
                Quaternion.identity).parent = instance.wave3_enemies;

            instance.wave2_enemies.gameObject.SetActive(false);
            instance.wave3_enemies.gameObject.SetActive(false);
        }

        // 스테이지 갱신
        instance.stageText.text = "STAGE " + instance.stageLevel;
    }    
}
