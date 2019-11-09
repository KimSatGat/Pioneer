using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {


    public GameObject TargetObj;


    public int dampValue = 10;
    public float ZPos = 10;
    public float yPos = 15.4f;
    public float MinXpos = -32f;
    public float MaxXPos = 35f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 TartgetPos = new Vector3(TargetObj.transform.position.x,  yPos, -ZPos);
        //Vector3 tmpPos = Vector3.Lerp(this.transform.position, TartgetPos, Time.deltaTime* dampValue);

        if (TartgetPos.x > MaxXPos)
        {
            TartgetPos = new Vector3(MaxXPos, yPos, -ZPos);

        }
        else if (TartgetPos.x < MinXpos)
        {

            TartgetPos = new Vector3(MinXpos, yPos, -ZPos);

        }
     

        this.transform.position = TartgetPos;

    }
}
