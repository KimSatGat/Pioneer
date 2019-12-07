using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobile_Input_Script : MonoBehaviour
{
    private string RoomCode; // 
    public string InputCode;
    public Text txt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void OpenKebord()
    {
        TouchScreenKeyboard.Open("",TouchScreenKeyboardType.Default);
    }

    public void OkayBtn()
    {
        InputCode = txt.text;
        Debug.Log(InputCode);


    }
}
