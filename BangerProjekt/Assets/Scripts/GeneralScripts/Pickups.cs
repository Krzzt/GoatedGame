using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    private int Typ; // what kind of pickup
    private float Duration; // the duration of the buff
    [SerializeField] private Player playerScript; // getting the player
    void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        Typ = Random.Range(0, 3); // determining the pickup typ
        Duration = 10.0f; 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // detecting if  colliding with a player
        {
            print("collision detected");
            print(Typ);
            playerScript.AddBuff(Typ, Duration); // giving the buff to the player
            print("added buff?");
            Destroy(gameObject); // saving some money by not wasting ram
        }
    }
}
