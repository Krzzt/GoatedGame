using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomList", menuName = "Game Data/Room List")]
public class RoomListSO : ScriptableObject
{
    public List<Room> allRooms = new List<Room>();

    public Room GetRoom(string id)
    {
        return allRooms.Find(r => r.roomID == id);
    }
}

