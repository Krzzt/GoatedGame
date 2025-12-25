using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{


    //Start of Card variables --------------------------------

    //End of Card variables ---------------------------------

    //Start of level variables ------------------------------
    private int level;
    private int currentExp = 0;
    private int requiredExp = 10;
    //End of level variables -------------------------------

    //Start of general Player variables ----------------------

     [NonSerialized] public int killCount; //THIS IS PUBLIC
    //like every enemy script wnats to access this it just makes sense

    //End of general Player variables -------------------------

    //_______________________________________________________________________________________________________________
    //START OF FUNCTIONS

    //Start of Unity specific functions ----------------------------
    void Awake()
    {
        CurrentHealth = MaxHealth; //set ur health
        for (int i = 0; i < 10; i++) // for testing, we initialize the entire deck with 5 cards of 2 different types
        {
            if (i % 2 == 0)
            {
                entireDeck.Add(CardList.allCards[0]);
            }
            else
            {
                entireDeck.Add(CardList.allCards[1]);
            }
        }
        drawPile = entireDeck; //all cards go into the drawPile
        ShuffleDrawPile();
        DrawCards(handSize); //we fill the hand with cards
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //if the collision is an enemy (as seen by its tag)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy>().Damage);
        }
    }
    //End of Unity specific functions ----------------------------


    //Start of HP related functions -----------------------------

    public event Action<int, int> OnHealthChanged;
    public void TakeDamage(int amount)
    {
        //This damage currently does not involve something like immunity frames or shit like that
        //also every enemy damages you on collision, if you hug them forever, you only take damage once!
        DamageUnit(amount);
        CurrentHealth -= amount;

        //Clamp the health to be between 0 and MaxHealth
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        Debug.Log("took damage!");

        //trigger the health changed event
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (MaxHealth <= 0)
        {
            Debug.Log("you should be dead");
            //Die
        }
    }

    public void Heal(int amount)
    {
        HealUnit(amount);
        
         //trigger the health changed event
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    //End of HP related functions --------------------------------

//events for exp changes
    public event Action<int, int> OnExpChanged;
    
    public event Action<int> OnLevelChanged;

    //Start of exp Related functions

    //getters for exp variables
    public int CurrentExp => currentExp;
    public int RequiredExp => requiredExp;
    public int Level => level;

    //events for exp changes
    public event Action<int, int> OnExpChanged;
    
    public event Action<int> OnLevelChanged;
    

    public void AddExp(int amount)
    {
        currentExp += amount;

        //trigger the exp changed event
        OnExpChanged?.Invoke(CurrentExp, RequiredExp);

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }
        //this while loop is here to make multiple level ups possible
    }

      
    public void LevelUp()
    {
        level++;
        requiredExp = (int)(requiredExp * 1.5f);
        //for now, this is just a number going up and the required exp also going up
        //trigger the level up event
        OnLevelChanged?.Invoke(Level);

        //stat increase probably
    }

    //end of exp related functions -----------------------

    //start of Pickup related functions ------------------
    public void AddBuff(int pickupType, float pickupDuration) 
    {
        
            switch (pickupType) // determinate the typ of pickup 
            {
                case 0: // Speed
                //print(MoveSpeed);
                   MoveSpeed *= 1.5f; //multiplying the players speed for the duration of the buff
                //print(MoveSpeed);
                    StartCoroutine(EndBuff(pickupType,pickupDuration));
                    break;
                case 1: // Strength
                //print(weaponScript.Damage);
                    weaponScript.Damage *= 2; //multiplying the players weapon dmg for the duration of the buff
                    StartCoroutine(EndBuff(pickupType,pickupDuration));
                //print(weaponScript.Damage);
                    break;
                case 2: // Hp
                    CurrentHealth += 20; // adding hp 
                    break;
            }
        
    }
    public IEnumerator EndBuff(int pickupType, float pickupDuration)
    {
        yield return new WaitForSeconds(pickupDuration); // removing buff on time over

        switch (pickupType)
        {
            case 0:
                MoveSpeed /= 1.5f; // removing the speed buff
                //print(MoveSpeed);
                 break;
            case 1:
                weaponScript.Damage /= 2; // removing the weapons dmg buff
                //print(weaponScript.Damage);
                 break;
        }
    }

    //end of Pickup related functions -------------------
    //start of inventory functions -----------------------
    public void ChangeItemStats(Item itemToChangeStats, bool addSub)
    {
        if (addSub)
        {
            weaponScript.Damage += itemToChangeStats.damage;
            weaponScript.FireRate += itemToChangeStats.fireRate; //because firerate is a frequency
            //defense not implemented
            AddMaxHealth(itemToChangeStats.healthBonus);
        }
        else
        {
            weaponScript.Damage -= itemToChangeStats.damage;
            weaponScript.FireRate -= itemToChangeStats.fireRate; //because firerate is a frequency
            //defense not implemented
            AddMaxHealth(-itemToChangeStats.healthBonus);
            //if equipment adds / subtracts more stats, this has to be added here
        }

    }
 //end of inventory functions
}