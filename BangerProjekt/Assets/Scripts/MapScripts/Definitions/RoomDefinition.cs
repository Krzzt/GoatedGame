using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room //Might become usefull somed day?
{
    public string roomName;
    public string roomID;
   //TODO: public biomeType biome; //Biome type of the room. The biome defines the assets used (only visual)
  
    

    public Room()
    {
        roomName = "DefaultRoom";
        roomID = "DefaultID";
    }


    public Room(string name, string id)
    {
        roomName = name;
        roomID = id;
    }
}