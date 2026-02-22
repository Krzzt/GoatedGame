using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    [field:SerializeField] public Transform DoorMiddle{get; set;} //The middle part of the door
    [field:SerializeField] public Transform DoorFacing{get; set;} //The facing part of the door
    [field:SerializeField] public Enums.DoorState State{get; set;} = Enums.DoorState.Hidden;
}


