using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{


    public int CurrentHealth{get;set;} //our current health via simple get/set

    [field:SerializeField] public int MaxHealth{get;set;} //Same for MaxHealth


    [field:SerializeField] public float MoveSpeed{get;set;}



    void Awake()
    {
        CurrentHealth = MaxHealth;
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
