using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    private int type; // what kind of pickup
    [SerializeField] private float duration; // the duration of the buff (SerializeField!)
    private Player playerScript; // getting the player
    [SerializeField] private List<Sprite> pickupSprites = new List<Sprite>();
    void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        type = Random.Range(0, 3); // determining the pickup type
        gameObject.GetComponent<SpriteRenderer>().sprite = pickupSprites[type];
        //duration = 10.0f; 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // detecting if colliding with a player (with Trigger, not collision)
        {
            playerScript.AddBuff(type, duration); // giving the buff to the player
            Destroy(gameObject); // saving some money by not wasting ram
            /*
            also we dont need the debug spam when we know it works for now
            Debug.Log("added buff?");
            Debug.Log("collision detected");
            Debug.Log(type);
            */
        }
    }
}
