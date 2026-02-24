using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
   [SerializeField] private List<GameObject> roomPrefabs; //List of all room prefabs available. Does not change during runtime (yet?)
   [SerializeField] private List<GameObject> rooms; //List of all rooms in the current layer
   [SerializeField] private List<GameObject> availableDoors; //List of all doors in the current layer
   [SerializeField] private List<GameObject> usedDoors; //List of all doors who have a valid room aligned
   [SerializeField] private GameObject startRoomPrefab; //The starting room prefab (Open for changes if necessary)
   [SerializeField] private int numOfRoomsInspector = 10; //Number of rooms to generate in the layer set by inspector taken as Default (Might become obsolete due to GenerateRooms being called from outside)
   [SerializeField] private int tries = 0; //Number of current tries (To prevent infinite Loops)
   [SerializeField] private int maxTries = 10000000; //Number of max Tries before the Loop breaks (To prevent infinite Loops)
   private GameObject startRoom; //The one and only start room instance


    public void Awake()
    {
       startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity); //Make tha start room
        float[] angles = { 0f, 90f, 180f, 270f }; //Simple array, because its not gonna change
        float randomAngle = angles[Random.Range(0, angles.Length)]; //Pick a random start rotation
        startRoom.transform.rotation = Quaternion.Euler(0, 0, randomAngle); // and apply it.
        rooms.Add(startRoom); //Yes exactly this room should be added to the rooms list
        availableDoors.Add(GameObject.FindWithTag("Door")); //And lets also get the first door.
    }

    [ContextMenu("Generate Rooms")] //To call GenerateRooms from the inspector (Will probably get obsolete once the Game Manager etc handles when to gen rooms)
    public void GenerateRooms() //Helper methode to be overriden that can be called from the inspector (Since it isn't possible to do so with a methode that has Parameters)
    {
        GenerateRooms(numOfRoomsInspector); //I use this to be able to default to the number set in the inspector if the call was not from an outside source.
    }
   public void GenerateRooms(int numOfRooms) //might later be called by something else, hence public and the Parameter(Optional)
   {
       for (int i = 0; i < numOfRooms; i++) //iterrate over how many rooms should be generated
       {
            tries++;
            if (tries >= maxTries)  //To Prevent infinite Loops (Yes it is a slight bottleneck if you want to create over 10k rooms (Who would do that?))
            {
                break;   
            } 
            
            if (availableDoors.Count == 0)break; //This implies that no start room has been generated so no place to start generation. There always has to be at least 1 door.
            int randomDoorIndex = Random.Range(0, availableDoors.Count); 
            GameObject randomDoor = availableDoors[randomDoorIndex];
 
            int randomIndex = Random.Range(0, roomPrefabs.Count); //Get a random index for the prefab list
            GameObject newRoom = Instantiate(roomPrefabs[randomIndex]); //Get the prefab with said random index
           
            List<GameObject> roomDoors = newRoom.GetComponent<RoomScript>().RoomDoors; //Gets the doors of the new room that has been instantiated. Rooms may have "infinite" doors.
            GameObject newRoomRandomDoor = roomDoors[Random.Range(0,roomDoors.Count)]; //Get the actual door we try to connect to
           
            if (TryPlaceRoom(randomDoor, newRoomRandomDoor)) //This calls with a random already existing door and the door we just picked returns Bool
            { //Successfully created a room:
                rooms.Add(newRoom); //Room added to the rooms list
                usedDoors.Add(availableDoors[randomDoorIndex]); //the doors that were used in the process. These shouldn't be used again
                usedDoors.Add(newRoomRandomDoor); //2. door =""=
                newRoomRandomDoor.GetComponent<DoorScript>().LinkDoor(availableDoors[randomDoorIndex].GetComponent<DoorScript>());
                newRoomRandomDoor.GetComponent<DoorScript>().LockDoor();
                availableDoors.RemoveAt(randomDoorIndex); //Remove the used door that already existed from the Available doors
                foreach (GameObject door in roomDoors) //Iterate over all new doors that were added with the room
                {
                    if(door != newRoomRandomDoor) //to not add the already used door
                    {
                        availableDoors.Add(door);//and add them to the available doors
                    }
                }

            }
            else
            { //Failed to create a room (due to overlap)
                i--; //add back to the counter of rooms to generate so we are not missing one
                Destroy(newRoom); //Discard the room that didn't fit and try again
            }

       }
       AddConnectedRooms();//If a random door has luckily aligned with another, we can have those set as "used" as well.
       //TODO: Let the doors know that they have been connected so they change their status from hidden to locked/open. Doors also are unable to be traversed at the moment.
       startRoom.GetComponent<RoomScript>().ClearRoom();
   }

   private void AlignRooms(GameObject doorA, GameObject doorB) //Now here comes the neat part
   {

        GameObject roomB = doorB.transform.parent.gameObject; //We only need the new room, the already existing room doesn't really matter

        Vector2 dirA = (doorA.GetComponent<DoorScript>().DoorFacing.position - doorA.GetComponent<DoorScript>().DoorMiddle.position).normalized; //We get the vectors of the doors middle to their facing points
        Vector2 dirB = (doorB.GetComponent<DoorScript>().DoorFacing.position - doorB.GetComponent<DoorScript>().DoorMiddle.position).normalized;

        float angleA = Mathf.Atan2(dirA.y, dirA.x) * Mathf.Rad2Deg; //Some math shit to get Angles
        float angleB = Mathf.Atan2(dirB.y, dirB.x) * Mathf.Rad2Deg;

        float targetRotation = (angleA + 180f) - angleB; //The rotation the room has to take so it aligns its door. 
        // The parentheses stay for my dyscalculate brain (Better explicit than implicit (Looking at you DeadLand and your random "void xxx" methodes))

        roomB.transform.rotation = Quaternion.Euler(0,0, targetRotation); //Lets rotate that bitch

        Vector3 currentDoorBPos = doorB.transform.position;  //get the positon of door B
        Vector3 displacement = doorA.transform.position - currentDoorBPos; //Now we only need to calculate how much we should move it by substracting its position from where it has to go

        roomB.transform.position += displacement; //and move it there by translating the movement to the room.

   }

   private bool IsOverlapping(GameObject room) //Overlap = Bad
    {
        CompositeCollider2D boundsCollider = room.GetComponentInChildren<CompositeCollider2D>(); //Every Bounds has one of these. The bounds are also the floor of the room.
        if (boundsCollider == null) return false; //There was no collider found :( That shouldn't happen.

        Collider2D[] results = new Collider2D[10]; //Max rooms to be overlapped with. This is a magic Literal because it doesn't really matter how big it is but 10 seemed fitting considering how big the rooms are.
        //This sorta limits how many rooms a single room can overlap with so we might or might not need to change this in the future.
        
        ContactFilter2D filter = new ContactFilter2D(); //Now lets filter to only look
        filter.SetLayerMask(LayerMask.GetMask("Background"));//for the specific layer
        filter.useTriggers = true; //and if the colliders are set to trigger. (If you use my Prefabs to build a room this should already be present)

        int found = boundsCollider.OverlapCollider(filter, results); //Now here comes the main line. This checks with the filter criteria if the bounds overlap with another and save all overlaps in the result array.
      
      for (int i = 0; i < found; i++) //Lets have a quick look into the array of overlapping rooms just to be sure we don't check the room with itself.
        {
            if (results[i].transform.root != room.transform.root) //Prevents to check if the room that is about to be placed is overlapping with itself
            {
                return true; //Then returns that an overlap was indeed found
            }
        }
        return false; //or if non was found
    }
   private bool TryPlaceRoom(GameObject doorA, GameObject doorB) //Let's try to place a room. YAY :D
    {
        AlignRooms(doorA, doorB); //First align those beautifully crafted rooms
        Physics2D.SyncTransforms(); //Force Unity to update the Transforms or it might take the old location
        if (IsOverlapping(doorB.transform.parent.gameObject)) //Let's see if it overlaps
        {
            return false; //It does. The TryPlaceRoom failed and it needs to be killed :(
        }
        return true; //It doesn't and the room was successfully placed to live a happy life :D
    }

  private void AddConnectedRooms() //If a random door has luckily aligned with another, we can have those set as "used" as well.
{
    HashSet<GameObject> doorsToRemove = new HashSet<GameObject>(); //HashSet to not have Duplicate doors

    for (int i = 0; i < availableDoors.Count; i++) //Iterate over Still available doors
    {
        for (int j = i + 1; j < availableDoors.Count; j++) // i+1 to prevent checking the door with itself
        {
            GameObject doorA = availableDoors[i]; //Get the doors
            GameObject doorB = availableDoors[j];

            if (Vector3.Distance(doorA.transform.position, doorB.transform.position) < 0.01f) //Check distance between doors if they are really close/overlap
            {
                doorsToRemove.Add(doorA); //Add doors to later remove into the HashSet
                doorsToRemove.Add(doorB);
                
                if(!usedDoors.Contains(doorA)) usedDoors.Add(doorA); //if for good measure that doors really not get added twice and are not already present in usedDoors
                if(!usedDoors.Contains(doorB)) usedDoors.Add(doorB);

                doorA.GetComponent<DoorScript>().LinkDoor(doorB.GetComponent<DoorScript>());
                doorA.GetComponent<DoorScript>().LockDoor();
                
                //Debug.Log($"Connected accidental overlap: {doorA.name} and {doorB.name}"); I dont know if we let those in or not
            }
        }
    }

    foreach (GameObject door in doorsToRemove) //Iterating over all doors in the HashSet
    {
        availableDoors.Remove(door); //remove doors fr fr
    }
}

}
