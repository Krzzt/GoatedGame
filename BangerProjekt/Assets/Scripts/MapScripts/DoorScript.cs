using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public Room adjecentRoom = null; //The room this door leads to
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//States a door can be in
public enum DoorState
{
    Open,
    Closed,
    Locked,
    Hidden
}
