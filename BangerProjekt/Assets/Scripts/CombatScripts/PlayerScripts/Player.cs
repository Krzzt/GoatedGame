using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Player : Unit
{

    private Weapon weaponScript;
    private UseAbilities abilityScript;
    [SerializeField] private GameObject fistPrefab;
    [SerializeField] private GameObject GameOverScreen;

    //Start of Card variables --------------------------------

    //End of Card variables ---------------------------------

    //Start of level variables ------------------------------
    private int level;
    private int currentExp = 0;
    private int requiredExp = 50;
    //End of level variables -------------------------------
    //Start of LifeSteal variables
    private bool IsStealingALife;
    private int LifeStealAmount = 1;
    //End of LifeSteal variables

    //Start of general Player variables ----------------------

    public int KillCount { get; set; }//THIS IS PUBLIC //Public Property bitch
    public int CurrImmunityFrames {get; private set;} //guess what that is
    public bool IsImmune { get; set;}

    [field: SerializeField] public int ImmuFramesOnHit; //how many frames of Immunity the player gets on hit (no shit sherlock)

    [field: SerializeField] public Class PlayerClass { get; set;}
    //End of general Player variables -------------------------

    //Start of Bonus Stat Variables (for now only Weapon) --------
    public int BonusDamage {get; private set;}
    public float BonusFireRate {get; private set;}
    //End of Bonus Stat Variables (for now only Weapon) -----------

    //Start of Item Variables -----------
    public static event Action<AbilityItem> NewAbility;
    public static event Action ToggleInventory;
    //End of Item Variables ------------

    //Interaction Event
    public static Action InteractEvent;
    //End of Interactuon Event

    //_______________________________________________________________________________________________________________
    //START OF FUNCTIONS

    //Start of Unity specific functions ----------------------------
    new void Awake()
    {   
        weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>(); //gameObject with small g = this.GameObject
        abilityScript = gameObject.GetComponent<UseAbilities>();
        base.Awake();


    }


    void OnCollisionEnter2D(Collision2D collision) //only calls if the collider collides with another collider (not trigger!!)
    {
        if (collision.gameObject.CompareTag("Enemy")) //if the collision is an enemy (as seen by its tag)
        {
            DamageUnit(collision.gameObject.GetComponent<Enemy>().Damage, 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!IsImmune && collision.gameObject.CompareTag("Enemy")) //IsImmune to not spam check tags
        {
            DamageUnit(collision.gameObject.GetComponent<Enemy>().Damage, 1);
        }
    }
    private void OnEnable()
    {
        InventoryLogic.ChangeItemPlayerStats += ChangeItemStats;
        InventoryLogic.SendNewWeapon += NewWeapon;
        SaveManager.SavingGame += SaveStats;
        SaveManager.LoadingGame += LoadStats;
        GameManager.currRoomChanged += RoomChange;
        RoomScript.RoomCleared += RoomChange;
    }

    private void OnDisable()
    {
        InventoryLogic.ChangeItemPlayerStats -= ChangeItemStats;
        InventoryLogic.SendNewWeapon -= NewWeapon;
        SaveManager.SavingGame -= SaveStats;
        SaveManager.LoadingGame -= LoadStats;
        GameManager.currRoomChanged -= RoomChange;
        RoomScript.RoomCleared -= RoomChange;

    }

    public void Interact()
    {
        InteractEvent?.Invoke();
    }

    //End of Unity specific functions ----------------------------


    //Start of HP related functions -----------------------------
    public override void DamageUnit(int amount, float crit)
    {
        if (IsImmune) return;
        if (amount <= 0) return;
        //This damage currently does not involve something like immunity frames or shit like that
        //also every enemy damages you on collision, if you hug them forever, you only take damage once!
        base.DamageUnit(amount, crit);
        AddImmunityFrames(ImmuFramesOnHit);
        PopUp.Create(transform.position + new Vector3(0.3f, 1.5f, 0), amount.ToString(), Color.red, 5);
        //Update the Healthbar if existent
        if (CurrentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        HealUnit(amount);
        //Update the healthbar if existent
    }

    public void ApplyLifesteal() // starts the life steal Attempted
    {
        if (!IsStealingALife) // if hasnt stolen a life for 0.1 sec
        {
            HealUnit(LifeStealAmount); // heals for 1 
            IsStealingALife = true; // Blocks other calls
            PopUp.Create(transform.position + new Vector3(0.3f, 1.5f, 0), "1", Color.green, 5); // the Green 1 pop up
            StartCoroutine(StartLifestealCooldown());
        }
    }

    public IEnumerator StartLifestealCooldown() // Starting the 0.1 Secound Cooldown
    {
        yield return new WaitForSeconds(0.1f); 
        IsStealingALife = false; // removing the LifeStealCD
    }
    public void Die()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    //End of HP related functions --------------------------------


    //Start of exp Related functions

    public void AddExp(int amount)
    {
        currentExp += amount;
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
        if (level % 2 == 0)
        {
            AddBonusDamage(1); //do 1 extra Damage
            AddBonusFireRate(0.1f); //slightly higher fireRate
        }
        AddMaxHealth(10); //get 10 max Health
        AddBonusFireRate(0.1f); //slightly higher fireRate
        PopUp.Create(transform.position + new Vector3(0.3f, 1.5f, 0), "Level Up!", Color.yellow, 7);
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
                    weaponScript.DamageMult++; //multiplying the players weapon dmg for the duration of the buff
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
                if (MoveSpeed < InitialMoveSpeed)
                {
                    MoveSpeed = InitialMoveSpeed;
                }
                //print(MoveSpeed);
                 break;
            case 1:
                weaponScript.DamageMult--; // removing the weapons dmg buff
                //print(weaponScript.Damage);
                 break;
            //case 2: doesnt exist because its a one time heal

        }
    }

    //end of Pickup related functions -------------------
    //start of inventory functions -----------------------
    public void ChangeItemStats(Item itemToChangeStats, bool addSub)
    {
        if (!itemToChangeStats) //to catch errors, see if an item even got sent
        {
            Debug.LogError("no item sent!");
            return;
        }
        if (addSub)
        {
            AddBonusDamage(itemToChangeStats.Damage);
            AddBonusFireRate(itemToChangeStats.FireRate);
            //defense not implemented
            AddMaxHealth(itemToChangeStats.HealthBonus);
            Debug.Log("Bonus DMG (ChangeItemStats): " + BonusDamage);
        }
        else
        {
            AddBonusDamage(-itemToChangeStats.Damage);
            AddBonusFireRate(-itemToChangeStats.FireRate);
            //defense not implemented
            AddMaxHealth(-itemToChangeStats.HealthBonus);
            //if equipment adds / subtracts more stats, this has to be added here
        }
        if (itemToChangeStats is AbilityItem)
        {
            AbilityItem tempAbility = itemToChangeStats as AbilityItem;
            abilityScript.Cooldown = tempAbility.AbilityCooldown;
            NewAbility?.Invoke(tempAbility);

        }



    }

    public void NewWeapon(GameObject newWeaponItem)
    {
        Debug.Log("NewWeapon called");
        if (!newWeaponItem)
        {
            newWeaponItem = fistPrefab;
        }
        Debug.Log("Destroying: " +GameObject.FindWithTag("Weapon").name);
        Destroy(GameObject.FindWithTag("Weapon")); //the weapon gets fucking blasted
        GameObject newWeaponObject = Instantiate(newWeaponItem, gameObject.transform);
        Debug.Log("newWeaponObject = " + newWeaponObject.name);
        weaponScript = newWeaponObject.GetComponent<Weapon>();
        weaponScript.Damage += BonusDamage;
        weaponScript.FireRate += BonusFireRate;
        //both 0 to just add the extra damage
        //simply adding that shit (might need to get a function later)
        //set new weapon and add stats 

    }
    public void AddBonusDamage(int amount)
    {
        weaponScript.Damage -= BonusDamage; //subtract so we can add everything at the end
        BonusDamage += amount;
        weaponScript.Damage += BonusDamage;
    }
    public void AddBonusFireRate(float amount)
    {
        weaponScript.FireRate -= BonusFireRate; //subtract so we can add everything at the end
        BonusFireRate += amount;
        weaponScript.FireRate += BonusFireRate;
    }
    //end of inventory functions

    //Saving/Loading Function
    private void SaveStats()
    {
        SaveManager.currentSave.EnemiesKilled = KillCount;
        SaveManager.currentSave.Level = level;
        SaveManager.currentSave.PlayerClass = PlayerClass;
    }

    private void LoadStats()
    {
        KillCount = SaveManager.currentSave.EnemiesKilled;
        level = SaveManager.currentSave.Level;
        PlayerClass = SaveManager.currentSave.PlayerClass;
        //literally just set everything from the Class
        InitialMoveSpeed = PlayerClass.StartingMoveSpeed;
        MoveSpeed += PlayerClass.StartingMoveSpeed;
        AddBonusDamage(PlayerClass.StartingBonusDamage);
        AddBonusFireRate(PlayerClass.StartingBonusFireRate);
        AddMaxHealth(PlayerClass.StartingHealth);
        Debug.Log("Bonus DMG (Loading)" + BonusDamage);
    }
    //End of Saving/Loading Function

    //Start of General Functions

    public void RoomChange() //everything the Player needs to do when the Room changes
    {
        if (GameManager.currentRoom.State == Enums.RoomState.Cleared)
        {
            MoveSpeed = InitialMoveSpeed * 1.8f; //80% increase
        }
        else
        {
            MoveSpeed = InitialMoveSpeed; //if its a new room, we fall back to our initial value
        }
    }

    public void AddImmunityFrames(int amount)
    {
        CurrImmunityFrames += amount;
        CancelInvoke("CountdownImmunityFrames"); //if it already runs, we dont want double countdown
        InvokeRepeating("CountdownImmunityFrames", 0, 0.02f); //fixed frames being 50/sec
    }

    public void CountdownImmunityFrames() //this is better than fixedUpdate because we only do it if it should, no if conditions 50/sec
    {
        if (CurrImmunityFrames > 0)
        {
            IsImmune = true;
            CurrImmunityFrames--;
        }
        else
        {
            IsImmune = false;
            CurrImmunityFrames = 0; //good measure
            CancelInvoke("CountdownImmunityFrames");
        }
    }
    public void toggleInventory()
    {
        ToggleInventory?.Invoke();
    }
    //End of General Functions
}
