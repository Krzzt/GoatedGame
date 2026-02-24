using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
   [field:SerializeField] public List<GameObject> RoomDoors{get; set;} //Doors this room has
   [field:SerializeField] public Enums.RoomState State {get; private set;} = Enums.RoomState.Uncleared; //The state of the room

    private void Awake() // used to prevent accidental overrides by the inspector
    {
        State = Enums.RoomState.Uncleared; //A room can never be cleared from the get go (Except the Start room which gets unlocked after room gen)
    }
   
    public bool IsCleared() //Returns a simple bool to check if the room is cleared. (Self explaining)
   {
      if(State == Enums.RoomState.Cleared)
      {
         return true;
      }
      return false;
   }
   [ContextMenu("Set Room As Cleared")] //Mostly for debug purposes and to replace still missing logic (Wave Manager)
   public void ClearRoom() //This name might change as clear room sounds a bit like it will be emptied but for now it just sets it as Cleared
   {
      State = Enums.RoomState.Cleared;
      foreach (GameObject door in RoomDoors)
      {
         if(door.GetComponent<DoorScript>().State == Enums.DoorState.Locked) //and unlocks the doors once it is
         {
            door.GetComponent<DoorScript>().OpenDoor();
         }
      }
   }

 
    public void OnRoomEnter() //Gets called by RoomBoundScript on each rooms bounds and that checks if the player is inside of the room
   {
      if (State == Enums.RoomState.Uncleared)
      {
         foreach (GameObject door in RoomDoors) //This locks the doors of the room so he may not leave until the room is cleared
         {
            if(door.GetComponent<DoorScript>().State == Enums.DoorState.Open)
            {
               door.GetComponent<DoorScript>().LockDoor();
            }
         }
      }
   }

}

