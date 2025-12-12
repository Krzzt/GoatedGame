using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitHealth //we do not inherit from "MonoBehaivour" here, so this is just a plain C# Class
{
    private int currentHealth;
    private int currentMaxHealth;
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
    public int CurrentMaxHealth //Same for maxHealth
    {
        get
        {
            return currentMaxHealth;
        }
        set
        {
            currentMaxHealth = value;
        }
    }

    public UnitHealth(int health, int maxHealth) //if we create an Object of class "UnitHealth" we need to set currentHealth and maxHealth
    {
        currentHealth = health;
        currentMaxHealth = maxHealth;

    }
    public void DamageUnit(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }
        //subtract damage Taken from the currentHealth
        //this Script does NOT handle dying!!!! Dying needs to be handled in different scripts

    }

    public void AddMaxHealth(int healthToAdd)
    {
        currentMaxHealth = healthToAdd + currentMaxHealth; //add to maxHealth
        MaxHealth = currentMaxHealth; //set the MaxHealth
        if (healthToAdd > 0)
        {
            currentHealth += healthToAdd; //currentHP also increase when maxHP increase. This does NOT happen when maxHP decrease
        }
            if (currentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            //afterwards check if currHP are higher than maxHP (especially for MaxHP DECREASES)
            }
        

    }
    public void HealUnit(int healAmount)
    {
        if (currentHealth < currentMaxHealth)
        {
            currentHealth += healAmount;
        }
        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
        //i dont even need to explain this
    }







}
