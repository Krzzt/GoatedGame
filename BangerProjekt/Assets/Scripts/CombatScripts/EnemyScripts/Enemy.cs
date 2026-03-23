using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : Unit
{
    [SerializeField] private GameObject xpObject;
    [SerializeField] private  int xpValue;
    [SerializeField] private GameObject pickup;
    [SerializeField] private float pickupDropChance; // setting the probabilty of dropping a pickup
    public float Distance{get;set;}
    protected GameObject playerObject;
    [field:SerializeField] public int Damage{get;set;}
    public Rigidbody2D RB {get; set;}
    protected Vector2 direction;
    protected Player playerScript;
    [field:SerializeField] public int Cost {get; set;}
    public static Action<GameObject> enemyDies;

    protected NavMeshPath pathToPlayer; //the NavMeshPath to calculate said path
    protected List<Vector3> nextMovePoint = new List<Vector3>(); //the points to move to saved in a List

    public new void Awake()
    {
        base.Awake();
        pathToPlayer = new NavMeshPath();
        RB = gameObject.GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();
    }


    public void Start()
    {
        InvokeRepeating("TurnToPlayer",0,0.2f);
        InvokeRepeating("MoveToPlayer",0,0.2f);
    }
    public void TurnToPlayer()
    {
        if (NavMesh.CalculatePath(transform.position, playerObject.transform.position, NavMesh.AllAreas, pathToPlayer)) //if we find a path
        {
            nextMovePoint = pathToPlayer.corners.ToList<Vector3>(); //we convert that shit into a list
        }
        else
        {
            Debug.LogError("No path to player found. You are fucked");
            //error no path found
        }
        Distance = Vector3.Distance(transform.position, playerObject.transform.position);
        direction = nextMovePoint[1] - transform.position;
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
        if (Vector3.Distance(transform.position, nextMovePoint[1]) < 0.05f) //if we are at a point
        {
            nextMovePoint.RemoveAt(1); //remove it to go to the next (not sure if necessary because it gets recalculated every frame)
        }
        //instead of transform, we use the rigidbody, this also helps for enemies that e.g have to dash
    }
    public bool ShouldPickupDrop() // the name
    {
        int temp = Random.Range(1, 101); 
        //Debug.Log(temp);
        if (temp <= pickupDropChance) // Here it calculates if the pickup should be dropped
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int amount)
    {
       DamageUnit(amount);
       PopUp.Create(gameObject.transform.position + new Vector3(0.3f,1.5f,0),amount.ToString(),Color.white);
       //Create a damage pop up (via the static function in the popup script)
       //Update health bar if existent
       if (CurrentHealth <= 0)
        {
            Die();
        } 
    }

    public virtual void Die()
    {
        playerObject.GetComponent<Player>().KillCount++; //killcount goes up by 1
        xpObject = Instantiate(xpObject, gameObject.transform.position, Quaternion.identity); //Create an XP GameObject and make it Addressable
        xpObject.GetComponent<XP>().Amount = xpValue;  //Edit the XP amount of the Created XP object instance
        if (ShouldPickupDrop())
        {
           pickup = Instantiate(pickup, gameObject.transform.position, Quaternion.identity); //Creating the Pickup
        }
        enemyDies?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
