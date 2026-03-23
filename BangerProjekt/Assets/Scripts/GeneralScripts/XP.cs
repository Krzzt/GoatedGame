using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
  public int Amount{get;set;}  
  [SerializeField] private Player playerScript;
    private const int FLYING_SPEED = 15;


    void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        RoomScript.RoomCleared += XPToPlayer;
    }

    private void OnDisable()
    {
        RoomScript.RoomCleared -= XPToPlayer;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //If the player Entered the Collection Radius
        {
            playerScript.AddExp(Amount); //Give Player XP
            Destroy(gameObject);  //Destroy Object
        } 
    }

    public void XPToPlayer()
    {
        InvokeRepeating("MoveToPlayer",0,0.02f);
    }

    public void MoveToPlayer()
    {
        Debug.Log("schmovin");
        transform.position = Vector2.MoveTowards(gameObject.transform.position, playerScript.gameObject.transform.position, Time.deltaTime * FLYING_SPEED);
    }
}
