using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingObject
{
    public Transform pivot;

    public Vector3 GetPivot()
    {
        return pivot.position;
    }
}
