using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static RoomScript currentRoom;
    public static Action<RoomScript> currRoomChanged;
    public static int roomsCleared = 0;
    public static int credits = 0; //This is money
    public static void SetCurrentRoom(RoomScript newRoom)
    {
        currentRoom = newRoom;
        currRoomChanged?.Invoke(currentRoom);
    }
}
