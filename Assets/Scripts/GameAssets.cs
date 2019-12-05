using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    public static GameAssets instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _instance;
        }
    }
    
    public Transform pfPopup;    
    public Transform pfPlayerMissile;
    public Transform pfEnemyMissile;
    public Transform pfDashEffect;

    public List<Transform> enemyList;
    public List<Sprite> abilitySprite;        

    
}
