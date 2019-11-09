using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {



    //[SerializeField]
    public Transform MainCamera;
    public int sortingOrder;
    public Material m_material;
	// Use this for initialization
	void Start () {
        MainCamera = Camera.main.gameObject.transform;
        //this.GetComponent<MeshRenderer>().sortingLayerName = "depth5";
       this.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        m_material = this.GetComponent<MeshRenderer>().material;
        StartCoroutine(MoveCloud());
	}
    public float ScrollSpeed = 0.5f;
    float Offset;


  

    IEnumerator MoveCloud()
    {
        while (true)
        {

            Offset += Time.deltaTime * ScrollSpeed*0.1f;
            m_material.mainTextureOffset = new Vector2(Offset, 0);


            //Vector2 tempPos = new Vector2(MainCamera.position.x, this.transform.position.y);
            //this.transform.position = tempPos;

           yield return new WaitForSeconds(0.1f);
        }

    }
}
