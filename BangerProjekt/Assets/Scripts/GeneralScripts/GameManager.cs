using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static RoomScript currentRoom;
    public static Action<RoomScript> currRoomChanged;
    public static int roomsCleared;
    public static int seed = 0;
    public static bool isSeeded = false;
    public static bool seedSet = false;
    public static int credits = 0; // Yay Money. WOOOOOO. (Name pending)
    [field:SerializeField] public List<Weapon> AllWeaponScripts {get; set;}
    public static GameManager Instance = null;

    void Awake()
    {
        
    if (Instance == null) //straight up copied from the discord (thx qutun)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <--- The Magic Line
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
    }
    public static void SetCurrentRoom(RoomScript newRoom)
    {
        currentRoom = newRoom;
        currRoomChanged?.Invoke(currentRoom);
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
    private void SaveGameManager()
    {
        SaveManager.currentSave.RoomsCleared = roomsCleared;
        SaveManager.currentSave.Seed = seed;
        SaveManager.currentSave.IsSeeded = true;
    }

    private void LoadGameManager()
    {
        roomsCleared = SaveManager.currentSave.RoomsCleared;
        seed = SaveManager.currentSave.Seed;
        isSeeded = SaveManager.currentSave.IsSeeded;
        credits = SaveManager.currentSave.credits;
    }

    public void Save()
    {
        StartCoroutine(SaveManager.SaveGame());
    }

    public void Load()
    {
        SaveManager.LoadGame();
    }
}
