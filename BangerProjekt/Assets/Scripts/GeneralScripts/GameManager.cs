using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public const int START_MIN_CREDITS = 10;
	public const int START_MAX_CREDITS = 20;
	public static RoomScript currentRoom;
	public static Action currRoomChanged;
	public static int roomsCleared;
	public static int seed = 0;
	public static bool isSeeded = false;
	public static bool seedSet = false;
	public static int credits = 0; // Yay Money. WOOOOOO. (Name pending)

	[SerializeField] private Class backupClass; //class that gets loaded if no save is found
	public static GameManager Instance = null;
	public static Action CreditsChanged;
	[field: SerializeField] public TMP_FontAsset GameFont { get; set; }
	[field: SerializeField] public GameObject LootChestPrefab { get; set; }

	void Awake()
	{
		Time.timeScale = 1.0f; //for good measure if the game starts, to not be in a paused state from GameOver or something else
		if (Instance == null) //straight up copied from the discord (thx qutun)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}
	private void Start()
	{
		//the gameManager Start is loaded after everything else so if something depends on the seed or other things being already loaded from the save in start, it needs to wait via coroutine
		Load();
		if (!isSeeded)
		{
			seed = Random.Range(0, 1000000000); //get a random seed
		}
		else //if seeded
		{
			seed = SaveManager.currentSave.Seed; //load that shit (is also loaded in LoadGameManager so not necessary)
		}
		Random.InitState(seed); //to actually set the seed
		seedSet = true;
		ChangeCredits(0); //to set the text
	}
	public static void SetCurrentRoom(RoomScript newRoom)
	{
		currentRoom = newRoom;
		currRoomChanged?.Invoke();
	}

	private void OnEnable()
	{
		SaveManager.SavingGame += SaveGameManager;
		SaveManager.LoadingGame += LoadGameManager;
	}
	private void OnDisable()
	{
		SaveManager.SavingGame -= SaveGameManager;
		SaveManager.LoadingGame -= LoadGameManager;
	}

	public void OnDestroy()
	{
		Instance = null;
		currentRoom = null;
		roomsCleared = 0;
		seed = 0;
		isSeeded = false;
		seedSet = false;
		credits = 0;
	}
	private void SaveGameManager()
	{
		SaveManager.currentSave.RoomsCleared = roomsCleared;
		SaveManager.currentSave.Seed = seed;
		SaveManager.currentSave.IsSeeded = true;
		SaveManager.currentSave.Credits = credits;
	}

	private void LoadGameManager()
	{
		roomsCleared = SaveManager.currentSave.RoomsCleared;
		seed = SaveManager.currentSave.Seed;
		isSeeded = SaveManager.currentSave.IsSeeded;
		credits = SaveManager.currentSave.Credits;
	}

	public void Save()
	{
		StartCoroutine(SaveManager.SaveGame());
	}

	public void Load()
	{
		SaveManager.LoadGame();
		if (!SaveManager.currentSave.PlayerClass)
		{
			SaveManager.currentSave.PlayerClass = backupClass;
		}
	}

	public void ChangeCredits(int amount)
	{
		credits += amount;
		if (credits < 0) credits = 0; //cant go in debt :(
		CreditsChanged?.Invoke();
		//we change the text here for now
	}
}
