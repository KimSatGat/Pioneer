using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDepthController : MonoBehaviour {


    public GameObject TargetObj;
    public float MoveValue = 2;

    private Vector3 OriginPos;
	// Use this for initialization
	void Start () {

        OriginPos = this.transform.position;
        OriginTargetPosX = TargetObj.transform.position.x;
        CurrentTargetPosX = OriginTargetPosX;


        StartCoroutine(UpdateMapDepth());
    }


    private float OriginTargetPosX;
    private float CurrentTargetPosX;
    public float TargetMoveDis;
    // Update is called once per frame
    //void Update () {

     


    //}


    IEnumerator UpdateMapDepth()
    {
        while (true)
        {
            CurrentTargetPosX = TargetObj.transform.position.x;

            TargetMoveDis = CurrentTargetPosX - OriginTargetPosX;

            //  float tmpvlu = TargetObj.transform.position.x * 0.01f;
            Vector3 tmpPos = new Vector3(OriginPos.x + (TargetMoveDis * MoveValue * 0.01f), OriginPos.y, OriginPos.z);

            this.transform.position = tmpPos;

            yield return new WaitForSeconds(0.1f);
        }

    }
}
