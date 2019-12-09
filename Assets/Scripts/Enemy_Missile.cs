using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Missile : MonoBehaviour
{    
    private float damage;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float gauge;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gauge = 0f;
    }

    private void SetDamage(float _damage)
    {
        damage = _damage;        
    }

    public void SetGauge(float _gague)
    {
        gauge += _gague;
    }

    public float GetGauge()
    {
        return gauge;
    }

    public void Fire(float _speed, Vector2 _moveDir)
    {        
        rb.AddForce(_speed * _moveDir);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {            
            Player player = other.gameObject.GetComponent<Player>();
            player.OnDamage(damage);

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
