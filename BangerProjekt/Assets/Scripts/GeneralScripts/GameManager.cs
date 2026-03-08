using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static RoomScript currentRoom;
    public static Action<RoomScript> currRoomChanged;
    public static int roomsCleared;
    public static void SetCurrentRoom(RoomScript newRoom)
    {
        currentRoom = newRoom;
        currRoomChanged?.Invoke(currentRoom);
    }

    private void OnEnable()
    {
        SaveManager.SavingGame += SaveCurrentRooms;   
        SaveManager.LoadingGame += LoadCurrentRooms;
    }
    private void OnDisable()
    {
        SaveManager.SavingGame -= SaveCurrentRooms;
        SaveManager.LoadingGame -= LoadCurrentRooms;
    }
    private void SaveCurrentRooms()
    {
        SaveManager.currentSave.RoomsCleared = roomsCleared;
    }

    private void LoadCurrentRooms()
    {
        roomsCleared = SaveManager.currentSave.RoomsCleared;
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
