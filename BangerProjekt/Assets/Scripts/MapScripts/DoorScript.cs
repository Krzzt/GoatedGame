using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public GameObject doorMiddle; //The middle part of the door
    public GameObject doorFacing; //The facing part of the door
    public Enums.DoorState state = Enums.DoorState.Hidden;
}


