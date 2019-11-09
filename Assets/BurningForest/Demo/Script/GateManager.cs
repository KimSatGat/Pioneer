using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour {

    

    //   Sword,Dagger,Spear,Punch,Bow,Gun,Grenade
    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("other::" + other);
        if (other.CompareTag("Player"))
        {
           // other.transform.position = StartPos.transform.position;
            other.transform.GetComponent<CharacterController_2D>().PressE_Obj.SetActive(true);
        }
            

    }

    void OnTriggerExit2D(Collider2D other)
    {

        Debug.Log("other::" + other);
        if (other.CompareTag("Player"))
        {
            // other.transform.position = StartPos.transform.position;
            other.transform.GetComponent<CharacterController_2D>().PressE_Obj.SetActive(false);
        }


    }

}
