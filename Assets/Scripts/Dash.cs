using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroyDash()
    {
        Color originColor = spriteRenderer.color;
        spriteRenderer.color = new Color(
            originColor.r,
            originColor.g,
            originColor.b,
            0f
            );
        Destroy(gameObject, 1f);
    }
}
