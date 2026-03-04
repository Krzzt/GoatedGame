using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static RoomScript currentRoom;
    public static Action<RoomScript> currRoomChanged;

    public static void SetCurrentRoom(RoomScript newRoom)
    {
        currentRoom = newRoom;
        currRoomChanged?.Invoke(currentRoom);
    }
}
