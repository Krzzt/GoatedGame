using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string roomName;
    public string roomID;
   //TODO public biomeType biome; //Biome type of the room. The biome defines the assets used (only visual)
    public List<GameObject> roomTypes; //Types or themes of this room
    public List<GameObject> doors; //Doors connected to this room
    

    public Room()
    {
        roomName = "DefaultRoom";
        roomID = "DefaultID";
        roomTypes = new List<GameObject>();
        doors = new List<GameObject>();
    }


    public Room(string name, string id)
    {
        roomName = name;
        roomID = id;
        roomTypes = new List<GameObject>();
        doors = new List<GameObject>();
    }

}