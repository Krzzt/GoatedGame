using System.Collections;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;


public class RoomBoundScript : MonoBehaviour
{
private CompositeCollider2D boundsCollider; //The collider of the parent of this script
private float safeDistance = -2f; //safety distance, how far you must enter the room to start it


private void Awake()
    {
        boundsCollider = GetComponent<CompositeCollider2D>(); //Set this collider
    }
void OnTriggerEnter2D(Collider2D player) //Checks if something enters the collider
    {
    if (player.gameObject.CompareTag("Player") && GetComponentInParent<RoomScript>().State == Enums.RoomState.Uncleared) 
    //Limits the loop logic to only run if it is a player entering and if the room hasnt been cleared.
    {
        InvokeRepeating("ProximityCheck",0,0.2f);
    }
    }

    void OnTriggerExit2D(Collider2D player) //Check if the player has left the room while all doors were still open
    {
        if (player.gameObject.CompareTag("Player"))
        {
            CancelInvoke("ProximityCheck");
        }
    }

    public void ProximityCheck()
    {
        if (CheckForFullEnter(GameObject.FindWithTag("Player").GetComponent<Collider2D>())) //Lets see if we are far enough in
        //also we can do this since we know this only gets called for the player and when the player collides in OnTriggerEnter
        {
            GetComponentInParent<RoomScript>().OnRoomEnter(); //Yes we were and now we can call this rooms OnRoomEnter
            CancelInvoke("ProximityCheck"); //and kill it
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
