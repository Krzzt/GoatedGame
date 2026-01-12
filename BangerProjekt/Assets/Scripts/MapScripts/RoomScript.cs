using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public List<DoorScript> roomDoors; //Doors this room has

    void Awake(){
        roomDoors = new List<DoorScript>(GetComponentsInChildren<DoorScript>());
    }


}

