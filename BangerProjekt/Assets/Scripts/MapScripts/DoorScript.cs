using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public Room adjecentRoom = null; //The room this door leads to
    public int doorId = 0;
    public Enums.DoorState state = Enums.DoorState.Hidden;
}


