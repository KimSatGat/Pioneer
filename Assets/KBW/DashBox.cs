using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBox : MonoBehaviour
{
    Melee_Player parent;
    
    void Start()
    {
        parent = GetComponentInParent<Melee_Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            parent.Dash();
        }
    }
}
