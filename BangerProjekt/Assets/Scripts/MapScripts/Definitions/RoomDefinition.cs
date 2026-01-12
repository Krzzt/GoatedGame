using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string roomName;
    public string roomID;
   //TODO: public biomeType biome; //Biome type of the room. The biome defines the assets used (only visual)
    public List<RoomScript> doors; //Doors connected to this room
    

    public Room()
    {
        roomName = "DefaultRoom";
        roomID = "DefaultID";
        doors = new List<RoomScript>();
    }


    public Room(string name, string id)
    {
        roomName = name;
        roomID = id;
        doors = new List<RoomScript>();
    }

}