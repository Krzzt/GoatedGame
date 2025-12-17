using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private int currentHealth;
    [SerializeField] private int maxHealth;
    //these are private and should only be touched via the "Health" and "MaxHealth" integers declared below
    public int CurrentHealth //our current health via simple get/set
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    public int MaxHealth //Same for maxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

       [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }


    void Awake()
    {
        currentHealth = maxHealth;
    }
    public void DamageUnit(int damageAmount)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damageAmount;
        }
        //subtract damage Taken from the currentHealth
        //this Script does NOT handle dying!!!! Dying needs to be handled in different scripts

    }

    public void AddMaxHealth(int healthToAdd)
    {
        MaxHealth = healthToAdd + MaxHealth; //add to MaxHealth
        if (healthToAdd > 0)
        {
            CurrentHealth += healthToAdd; //currentHP also increases when maxHP increases. This does not happen when maxHP decreases
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    public void HealUnit(int healAmount)
    {
        if (CurrentHealth + healAmount < MaxHealth) //check for overheal before healing
        {
            CurrentHealth += healAmount;
        }
        else CurrentHealth = MaxHealth;
    }


}
