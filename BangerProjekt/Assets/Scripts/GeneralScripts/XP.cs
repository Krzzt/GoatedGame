using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
  public int Amount{get;set;}  
  [SerializeField] private Player playerScript;
  

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Player")) //If the player Entered the Collection Radius
    {
      playerScript.AddExp(Amount); //Give Player XP
      Destroy(gameObject);  //Destroy Object
    } 
  }
}
