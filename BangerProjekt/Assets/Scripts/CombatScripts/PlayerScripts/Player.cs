using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public UnitHealth PlayerHealth = new UnitHealth(100); //Player starts with 100HP (just example, can be changed anytime)





    public void TakeDamage(int amount)
    {
        PlayerHealth.DamageUnit(amount);
        if (PlayerHealth.MaxHealth <= 0)
        {
            //Die
        }
    }

    public void Heal(int amount)
    {
        PlayerHealth.HealUnit(amount);
    }
}
