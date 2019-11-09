using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthManager : MonoBehaviour {



    private float Update_Tic = 0;
    private float Update_Time = 0.1f;

    public SpriteRenderer[] m_SpriteGroup;
    public GameObject Player;


    public float PlayerZ;
 

    void Start()
    {
    
        m_SpriteGroup = this.transform.GetComponentsInChildren<SpriteRenderer>(true);
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerZ = this.transform.position.z;
       

    }

    void Update()
    {
        spriteOrder_Controller();
    }
    void spriteOrder_Controller()
    {

        Update_Tic += Time.deltaTime;

        if (Update_Tic > 0.1f)
        {

            float disY = this.transform.position.y - Player.transform.position.y;

            if (disY > 0)
            {
               
             ///   Debug.Log("위");

                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,-3);


            }
            else
            {
                
                //Debug.Log("아래");
                //Debug.Log("y::" + this.transform.position.y);
                //  Debug.Log("sortingOrder::" + sortingOrder);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -30);
            }

            Update_Tic = 0;
        }



    }
}
