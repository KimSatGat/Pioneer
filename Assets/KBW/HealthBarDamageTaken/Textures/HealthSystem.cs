using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{   
    private int healthAmount;
    private int healthAmountMax;
    public event Action OnDamaed;
    public event Action OnHealed;

    public HealthSystem(int healthAmount)
    {
        this.healthAmount = healthAmount;
        healthAmountMax = healthAmount;
    }

    public void Damage(int amount)
    {
        healthAmount -= amount;
        if(healthAmount < 0)
        {
            healthAmount = 0;
        }
        
        if(OnDamaed != null)
        {
            OnDamaed();
        }
    }

    public void Heal(int amount)
    {
        healthAmount += amount;
        if(healthAmount > healthAmountMax)
        {
            healthAmount = healthAmountMax;
        }        

        if(OnHealed != null)
        {
            OnHealed();
        }
    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / (float)healthAmountMax;
    }

    public void SethealthAmount(int hp)
    {
        healthAmount = hp;
    }
}
