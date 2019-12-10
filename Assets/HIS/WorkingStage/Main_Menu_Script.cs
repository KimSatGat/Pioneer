using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu_Script : MonoBehaviour
{
    public AudioSource MainBGM;
    public AudioClip UI_Sound;

    public Slider BGMslider;
    public Slider IGMslider;

    private float BGM_slide_SetValue;
    public float IGM_slide_SetValue;

    private void Awake()
    {
        Load_Data();
    }

    // Start is called before the first frame update
    void Start()
    {
        BGMslider.value = BGM_slide_SetValue;

    }

    // Update is called once per frame
    void Update()
    {
        MainBGM.volume = 1f - BGMslider.value;
        
    }
    public void Game_Start()
    {
        UI_Btn_Sound();

        Debug.Log("Start");
        SceneManager.LoadScene("Main");
    }
    public void Game_Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void Go_MainMenu()
    {

    }
    public void TouchKebord()
    {
        TouchScreenKeyboard.Open("친구의 코드를 입력하세요.", TouchScreenKeyboardType.Default);
    }
    public void Accept_btn()
    {
        UI_Btn_Sound();

        BGM_slide_SetValue = BGMslider.value;
        PlayerPrefs.SetFloat("BGM_Set", BGM_slide_SetValue);

        IGM_slide_SetValue = IGMslider.value;
        PlayerPrefs.SetFloat("IGM_Set", IGM_slide_SetValue);

    }
    public void Load_Data()
    {
        BGM_slide_SetValue = PlayerPrefs.GetFloat("BGM_Set", 0);
        IGM_slide_SetValue = PlayerPrefs.GetFloat("IGM_Set", 0);
    }
    public void UI_Btn_Sound()
    {
        MainBGM.PlayOneShot(UI_Sound, 1f);
    }
}
