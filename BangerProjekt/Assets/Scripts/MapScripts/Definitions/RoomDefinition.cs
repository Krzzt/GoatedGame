using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string roomName;
    public string roomID;
<<<<<<< HEAD
   //TODO public biomeType biome; //Biome type of the room. The biome defines the assets used (only visual)
    public List<GameObject> roomTypes; //Types or themes of this room
    public List<GameObject> doors; //Doors connected to this room
=======
   //TODO: public biomeType biome; //Biome type of the room. The biome defines the assets used (only visual)
    public List<RoomScript> doors; //Doors connected to this room
>>>>>>> refs/remotes/origin/TLG-rooms-and-maps
    

    public Room()
    {
        roomName = "DefaultRoom";
        roomID = "DefaultID";
<<<<<<< HEAD
        roomTypes = new List<GameObject>();
        doors = new List<GameObject>();
=======
        doors = new List<RoomScript>();
>>>>>>> refs/remotes/origin/TLG-rooms-and-maps
    }


    public Room(string name, string id)
    {
        roomName = name;
        roomID = id;
<<<<<<< HEAD
        roomTypes = new List<GameObject>();
        doors = new List<GameObject>();
=======
        doors = new List<RoomScript>();
>>>>>>> refs/remotes/origin/TLG-rooms-and-maps
    }

}