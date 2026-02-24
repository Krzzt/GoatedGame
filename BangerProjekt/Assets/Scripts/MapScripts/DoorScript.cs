using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [field:SerializeField] public Transform DoorMiddle{get; set;} //The middle part of the door
    [field:SerializeField] public Transform DoorFacing{get; set;} //The facing part of the door
    [field:SerializeField] public Enums.DoorState State{get; set;} = Enums.DoorState.Hidden; //Ohio
    [field:SerializeField] private DoorScript linkedDoor; //The door that is connected to this one


    void Awake()
    {
        PaintDoor(); //So the doors are painted right once they spawn
    }
    public void LinkDoor(DoorScript door) //Link the 2 doors that belong together
    {
        if (linkedDoor != door) //and link back so you only need to call link on one door
        {
            linkedDoor = door;
            door.LinkDoor(this);
        }
        PaintDoor(); //Lets paint over it again for good measure (since their state might have changed)
        
    }
 [ContextMenu("OpenDoor")]
    public void OpenDoor() //Open ni noor
    {
        State = Enums.DoorState.Open;
        linkedDoor.State = Enums.DoorState.Open;
        gameObject.GetComponent<BoxCollider2D>().enabled = false; //make it passable
        linkedDoor.gameObject.GetComponent<BoxCollider2D>().enabled = false; //and even on the linked door
        PaintDoor(); //Paint paint paint
        linkedDoor.PaintDoor(); //Some more painting
    }
        public void LockDoor() //Locked up and ready
    {
        State = Enums.DoorState.Locked;
        linkedDoor.State = Enums.DoorState.Locked;
        gameObject.GetComponent<BoxCollider2D>().enabled = true; //make it not passable
        linkedDoor.gameObject.GetComponent<BoxCollider2D>().enabled = true;//and even on the linked door
        PaintDoor(); //Phew it gets tiring slowly
        linkedDoor.PaintDoor(); //Ohh no my paint brush hurts
    }

    public void PaintDoor() //The root of all the art
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>(); //Render it

        switch (State)
        {
        case Enums.DoorState.Hidden:
            renderer.color = new Color32(140,70,70,255); // The wall color
        break;

        case Enums.DoorState.Locked:
            renderer.color = new Color32(255,211,109,255); // The usual yellow
        break;

        case Enums.DoorState.Open:
            renderer.color = new Color32(131,235,110,255); // A soft green
        break;

        default:
            Debug.LogWarning("Door is in an unknown state!"); // AAAHHH THE DOOR HAS BECOME SENTIENT
        break;
}
        
    }
}


