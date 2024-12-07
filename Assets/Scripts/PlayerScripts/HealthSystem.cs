using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

    public float healthAmount;
    public float healthAmountMax;

    public void SetUp(float healthAmount)
    {
        healthAmountMax = healthAmount;
        this.healthAmount = healthAmount;

    }

    public void Damage(float amount)
    {

        healthAmount -= amount;
        if (healthAmount < 0)
        {
            healthAmount = 0;
        }
        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

    }

    public void Heal(float amount)
    {
        healthAmount += amount;
        if (healthAmount > healthAmountMax)
        {
            healthAmount = healthAmountMax;
        }
        if (OnHealed != null) OnHealed(this, EventArgs.Empty);

    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }

}
