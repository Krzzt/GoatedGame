using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> allRooms = new List<Room>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom();
    }

    // Update is called once per frame
   /* void FixedUpdate()
    {
        if(allRooms.Count == 0)
        {
            GenerateRoom();
        }

    }*/
    void GenerateRoom()
    {
        Room room = new Room("testRoom", "ThisIsATestRoomID");
        allRooms.Add(room);
    }

    void paintMap()
    {
        foreach(Room room in allRooms)
        {
            
        }
    }
}
