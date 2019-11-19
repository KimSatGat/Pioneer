using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Missile : MonoBehaviour
{
    private float damage;
    private float speed;
    private Vector2 moveDir;
    private Rigidbody2D rigidbody2D;    

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.AddForce(moveDir * speed);
    }
            
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            LivingObject livingObject = collision.GetComponent<LivingObject>();
            livingObject.OnDamage(damage);

            Destroy(gameObject);
        }
    }  

    public static Player_Missile Create(Vector2 _createPos, Vector2 _moveDir, float _speed, float _damage)
    {
        Transform playerMissileTransform = Instantiate(GameAssets.instance.pfPlayerMissile, _createPos, Quaternion.identity);

        Player_Missile player_Missile = playerMissileTransform.GetComponent<Player_Missile>();

        player_Missile.moveDir = _moveDir;
        player_Missile.damage = _damage;
        player_Missile.speed = _speed;

        return player_Missile;
    }    
}
