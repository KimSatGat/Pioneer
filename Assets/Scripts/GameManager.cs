using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public Transform[] spawnList;

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

    public static int stageLevel = 1;
    int maxMonsterCount = 12;
    int currentMonsterCount = 3;

    ArrayList enemies = new ArrayList();

    private void Awake()
    {
        SettingResolution();
    }

    private void Start()
    {        
        for (int i = 0; i < currentMonsterCount; i++)
        {
            int idx = Random.Range(0, 5);
            enemies.Add(Instantiate(GameAssets.instance.enemyList[idx], spawnList[i].position, Quaternion.identity));
        }
    }

    private void Update()
    {
        stageText.text = "STAGE " + stageLevel;

        
            //Debug.Log("트리거 활성화");        
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
}
