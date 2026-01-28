using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeIndicatorDamageGiver : MonoBehaviour
{
   public bool PlayerIsInRange {get; set;}
    //with this script we just check if the player is in range of our specific bomboclat
    void Awake()
    {
        PlayerIsInRange = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerIsInRange = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision) //if the player is in range when bomboclat starts, this is a must
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerIsInRange = true;
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerIsInRange = false;
        } 
    }
}
