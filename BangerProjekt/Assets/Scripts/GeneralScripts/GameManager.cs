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

    public void Awake()
    {
        Load();
        if (!isSeeded)
        {
            seed = Random.Range(0, 1000000000);
            Random.InitState(seed);
        }
        else //if seeded
        {
            seed = SaveManager.currentSave.Seed; //load that shit (is also loaded in LoadGameManager so not necessary)
        }
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
