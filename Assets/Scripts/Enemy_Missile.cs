using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Missile : MonoBehaviour
{    
    private float damage;
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SetDamage(float _damage)
    {
        damage = _damage;        
    }

    public void SetColor(Color _color)
    {
        spriteRenderer.color += _color;
    }

    public float GetColorAlpha()
    {
        return spriteRenderer.color.a;
    }

    public void Fire(float _speed, Vector2 _moveDir)
    {        
        rigidbody2D.AddForce(_speed * _moveDir);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LivingObject livingObject = collision.GetComponent<LivingObject>();
            livingObject.OnDamage(damage);

            Destroy(gameObject);
        }
    }
    
    public static Enemy_Missile Create(Vector2 CreatePos, float _damage)
    {
        Transform missileTransform = Instantiate(GameAssets.instance.pfEnemyMissile, CreatePos, Quaternion.identity);
        Enemy_Missile missile = missileTransform.GetComponent<Enemy_Missile>();
        missile.SetDamage(_damage);
        
        return missile;
    }   
}
