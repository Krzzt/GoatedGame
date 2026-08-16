using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : Unit
{

	public static Player Instance;
	private Weapon weaponScript;
	private UseAbilities abilityScript;
	[SerializeField] private GameObject fistPrefab;
	[field: SerializeField] public GameObject GameOverScreen { get; set; }

	//Start of Card variables --------------------------------

	//End of Card variables ---------------------------------

	//Start of level variables ------------------------------
	public int Level { get; private set; } = 1;
	public int CurrentExp { get; private set; } = 0;
	public int RequiredExp { get; private set; } = 50;
	//End of level variables -------------------------------
	//Start of LifeSteal variables
	private bool IsStealingALife;
	private int LifeStealAmount = 1;
	private float lifeStealCooldown = 0.1f;
	//End of LifeSteal variables

	//Start of general Player variables ----------------------

	public int KillCount { get; set; }//THIS IS PUBLIC //Public Property bitch
	public int CurrImmunityFrames { get; private set; } //guess what that is
	public bool IsImmune { get; set; }

	[field: SerializeField] public int ImmuFramesOnHit; //how many frames of Immunity the player gets on hit (no shit sherlock)

	[field: SerializeField] public Class PlayerClass { get; set; }

	//End of general Player variables -------------------------

	//Start of Bonus Stat Variables --------
	[field: SerializeField] public float BonusDamage { get; set; }
	[field: SerializeField] public float BonusFireRate { get; set; }
	[field: SerializeField] public float BonusSpreadAngle { get; set; }
	[field: SerializeField] public int BonusBulletAmount { get; set; }
	[field: SerializeField] public float BonusShotSpeed { get; set; }
	[field: SerializeField] public float BonusMoveSpeed { get; set; }
	[field: SerializeField] public float BonusCritChance { get; set; }
	[field: SerializeField] public float BonusCritDamage { get; set; }

	private int bonusZoom;
	public int BonusZoom
	{
		get
		{
			return bonusZoom;
		}
		set
		{
			GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize -= bonusZoom;
			bonusZoom = value;
			GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize += bonusZoom;
		}
	}
	[field: SerializeField] public int BonusPierce { get; set; }
	//End of Bonus Stat Variables -----------

	//Start of Item Variables and Actions -----------
	public static event Action<AbilityItem> NewAbility;
	public static event Action ToggleInventory;
	public static event Action ToggleShop;
	public static event Action TogglePauseMenu;
	public static event Action Die;
	//End of Item Variables and Actions ------------

	//Interaction Event
	public static PlayerInput playerInput { get; set; } //static way to get the player input from wherever its needed. Much more efficient than searching for the player.
	public static Action InteractEvent;
	//End of Interactuon Event

	//_______________________________________________________________________________________________________________
	//START OF FUNCTIONS

	//Start of Unity specific functions ----------------------------
	new void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>(); //gameObject with small g = this.GameObject
		abilityScript = gameObject.GetComponent<UseAbilities>();
		playerInput = this.GetComponent<PlayerInput>();
		GameOverScreen.SetActive(false);
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

	public void OnDestroy()
	{
		Instance = null;
		playerInput = null;
	}

	//End of Unity specific functions ----------------------------


	//Start of HP related functions -----------------------------
	public override void DamageUnit(int amount, float crit)
	{
		if (IsImmune) return;
		if (amount <= 0) return;
		if (CurrentHealth > 0)
		{
			amount = Mathf.RoundToInt(amount * (1 - DamageReduction)); // calculates damage ammount based on Damage Reduction
			CurrentHealth -= amount;
		}
		AddImmunityFrames(ImmuFramesOnHit);
		PopUp.Create(transform.position + new Vector3(0.3f, 1.5f, 0), amount.ToString(), Color.red, 5);
		//Update the Healthbar if existent
		if (CurrentHealth <= 0) Die?.Invoke();
	}

	public override void HealUnit(int amount)
	{
		base.HealUnit(amount);
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

	public IEnumerator StartLifestealCooldown() // Starting the Cooldown
	{
		yield return new WaitForSeconds(lifeStealCooldown);
		IsStealingALife = false; // removing the LifeStealCD
	}

	//End of HP related functions --------------------------------


	//Start of exp Related functions

	public void AddExp(int amount)
	{
		CurrentExp += amount;
		while (CurrentExp >= RequiredExp)
		{
			CurrentExp -= RequiredExp;
			LevelUp();
		}
		//this while loop is here to make multiple level ups possible
	}

	public void LevelUp()
	{
		Level++;
		RequiredExp = (int)(RequiredExp * 1.5f);
		if (Level % 2 == 0)
		{
			BonusDamage += 0.1f; //10% bonus dmg
			BonusFireRate += 0.1f; //10% bonus firerate
		}
		AddMaxHealth(10); //get 10 max Health
		BonusFireRate += 0.1f; //10% bonus firerate
		PopUp.Create(transform.position + new Vector3(0.3f, 1.5f, 0), "Level Up!", Color.yellow, 7);
		//stat increase probably
	}

	//end of exp related functions -----------------------

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
			BonusDamage += itemToChangeStats.Damage;
			BonusFireRate += itemToChangeStats.FireRate;
			BonusBulletAmount += itemToChangeStats.BulletAmount;
			BonusCritChance += itemToChangeStats.CritChance;
			BonusCritDamage += itemToChangeStats.CritDamage;
			Defense += itemToChangeStats.Defense;
			AddMaxHealth(itemToChangeStats.HealthBonus);
		}
		else
		{
			BonusDamage -= itemToChangeStats.Damage;
			BonusFireRate -= itemToChangeStats.FireRate;
			BonusBulletAmount -= itemToChangeStats.BulletAmount;
			BonusCritChance -= itemToChangeStats.CritChance;
			BonusCritDamage -= itemToChangeStats.CritDamage;
			Defense -= itemToChangeStats.Defense;
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
		if (!newWeaponItem)
		{
			newWeaponItem = fistPrefab;
		}
		Destroy(GameObject.FindWithTag("Weapon")); //the weapon gets fucking blasted
		GameObject newWeaponObject = Instantiate(newWeaponItem, gameObject.transform);
		weaponScript = newWeaponObject.GetComponent<Weapon>();
		//both 0 to just add the extra damage
		//simply adding that shit (might need to get a function later)
		//set new weapon and add stats

	}

	//end of inventory functions

	//Saving/Loading Function
	private void SaveStats()
	{
		SaveManager.currentSave.EnemiesKilled = KillCount;
		SaveManager.currentSave.Level = Level;
		SaveManager.currentSave.PlayerClass = PlayerClass;
	}

	private void LoadStats()
	{
		KillCount = SaveManager.currentSave.EnemiesKilled;
		Level = SaveManager.currentSave.Level;
		PlayerClass = SaveManager.currentSave.PlayerClass;
		//literally just set everything from the Class
		InitialMoveSpeed = PlayerClass.StartingMoveSpeed;
		MoveSpeed += PlayerClass.StartingMoveSpeed;
		BonusDamage += PlayerClass.StartingBonusDamage;
		BonusFireRate += PlayerClass.StartingBonusFireRate;
		AddMaxHealth(PlayerClass.StartingHealth);
		BonusShotSpeed += PlayerClass.StartingBonusShotSpeed;
		BonusMoveSpeed += PlayerClass.StartingBonusMoveSpeed;
		BonusSpreadAngle = 1f; //standard 100%
		BonusCritChance += PlayerClass.StartingBonusCritChance;
		BonusCritDamage += PlayerClass.StartingBonusCritDamage;
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
		IsImmune = true; //we prevent same frame hits with this
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
	public void toggleInventory() { ToggleInventory?.Invoke(); }
	public void toggleShop() { ToggleShop?.Invoke(); }
	public void togglePauseMenu() { TogglePauseMenu?.Invoke(); }
	public int CalcTotalDamage()
	{
		return (int)Math.Round(weaponScript.Damage * BonusDamage);
	}
	//End of General Functions
}
