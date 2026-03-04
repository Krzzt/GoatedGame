using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class RoomBoundScript : MonoBehaviour
{
private Coroutine safetyCheck; //To check if the player is far enough away from the door to safely shut it without clipping.
private CompositeCollider2D boundsCollider; //The collider of the parent of this script
private float safeDistance = -2f; //safety distance, how far you must enter the room to start it

private void Awake()
    {
        boundsCollider = GetComponent<CompositeCollider2D>(); //Set this collider
    }
void OnTriggerEnter2D(Collider2D player) //Checks if something enters the collider
{
    if (player.gameObject.CompareTag("Player") && safetyCheck == null && transform.root.GetComponent<RoomScript>().State == Enums.RoomState.Uncleared) 
    //Limits the loop logic to only run if it is a player entering, it isn't already running and if the room hasnt been cleared.
    {
        safetyCheck = StartCoroutine(ProximityCheck(player)); //Start the check loop
    }
}

    void OnTriggerExit2D(Collider2D player) //Check if the player has left the room while all doors were still open
    {
        if (player.gameObject.CompareTag("Player") && safetyCheck != null) //and only kill the coroutine if it is running and the trigger caused by a player
        {
            StopCoroutine(safetyCheck); //Kill it with fire
            safetyCheck = null; //and discard its corpse
        }
        
    }

    IEnumerator ProximityCheck(Collider2D player) //Coroutine
{
    while (true) //Now for real start that shit
    {
       
        if (CheckForFullEnter(player)) //Lets see if we are far enough in
        {
            GetComponentInParent<RoomScript>().OnRoomEnter(); //Yes we were and now we can call this rooms OnRoomEnter
            safetyCheck = null; //And reset this check so it might run again if the room hasn't been cleared
            yield break; //and now leave
        }

        yield return new WaitForSeconds(0.1f); // Check 10 times a second
    }
}

private bool CheckForFullEnter(Collider2D player)
    {
        //bool isOnBounds = boundsCollider.OverlapPoint(player.transform.position);
        ColliderDistance2D distance = boundsCollider.Distance(player); //Now thats some distance
        //Debug.Log(distance.distance);
        
        if(distance.distance < safeDistance) //lets check if the Player is deep enough into the room
        {
            //Debug.Log("safe distance reached");
           return true; //Now if we are safe we can return true
        }
        return false; //and return false if it is not safe to close the door
    }

}
//Why didnt i use OnTriggerStay? I was stupid, thats why but i dont wanna rewrite everything i did so this is fine for now.
//Besides, it only checks 10 times per second while OnTriggerStay would fire every fixed update so 50 times per second. (Saves some computing)
