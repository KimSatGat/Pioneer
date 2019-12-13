using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameMenu_script :Main_Menu_Script
{
    public AudioSource IGM;
    

    bool bPause;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IGM.volume = 1f - IGM_slide_SetValue;
    }
    public void Pause()
    {
        UI_Btn_Sound();
        Time.timeScale = 0;
    }
    public void Continue_btn()
    {
        UI_Btn_Sound();
        Time.timeScale = 1f;
    }
    public void Restart_btn()
    {
        UI_Btn_Sound();
        //SceneManager.LoadScene("His");
        SceneManager.LoadScene("Main");
        Time.timeScale = 1f;
    }

    public void Quit_btn()
    {
        UI_Btn_Sound();
        Application.Quit();
    }
    public void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            bPause = true;
        }
        else
        {
            if(bPause)
            {
                bPause = false;
            }
        }
    }
}
