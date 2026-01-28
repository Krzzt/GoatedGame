using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private GameObject xpObject;
    [SerializeField] private  int xpValue;

    public float Distance{get;set;}

    private GameObject playerObject;

    [field:SerializeField] public int Damage{get;set;}

    public Rigidbody2D RB {get; set;}
    private Vector2 direction;

    protected Player playerScript;




    public void Awake()
    {
        CurrentHealth = MaxHealth;
        RB = gameObject.GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();
    }
    public void FixedUpdate()
    {
        TurnToPlayer();
        MoveToPlayer();
    }
    public void TurnToPlayer()
    {
        Distance = Vector2.Distance(transform.position, playerObject.transform.position);
        direction = playerObject.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0,0,-90);
        //turning stuff
        //basically we get the distance between enemy and player but as a Vector and then we turn that vector into an angle
    }
    public void MoveToPlayer()
    {

        //transform.position = Vector2.MoveTowards(gameObject.transform.position, playerObject.transform.position, MoveSpeed * Time.fixedDeltaTime);
        RB.velocity = direction * MoveSpeed;
        //instead of transform, we use the rigidbody
    }


    public void TakeDamage(int amount)
    {
       DamageUnit(amount);
       PopUp.Create(gameObject.transform.position + new Vector3(0.3f,1.5f,0),amount.ToString(),Color.white);
       //Create a damage pop up (via the static function in the popup script)
       //Update health bar if existent
       if (CurrentHealth <= 0)
        {
            Debug.Log("Enemy ded");
            playerObject.GetComponent<Player>().KillCount++; //killcount goes up by 1
            xpObject = Instantiate(xpObject, gameObject.transform.position, Quaternion.identity); //Create an XP GameObject and make it Addressable
            xpObject.GetComponent<XP>().Amount = xpValue;  //Edit the XP amount of the Created XP object instance
            Destroy(gameObject);
        } 
    }
}
