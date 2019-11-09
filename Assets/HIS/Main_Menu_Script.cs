using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene("Game_Start");
        
    }
    public void Game_Start()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("WorkingStage_HIS");
    }
    public void Game_Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
