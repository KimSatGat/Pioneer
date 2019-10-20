using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        SettingResolution();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 해상도 설정
    void SettingResolution()
    {
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
    }
}
