using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
     [SerializeField] private GameObject xpObject;
    [SerializeField] private  int xpValue;
    [SerializeField] private GameObject pickup;
    [SerializeField] private float pickupDropChance; // setting the probabilty of dropping a pickup

    [SerializeField] private readonly int minXP;
    [SerializeField] private readonly int maxXP; //exclusive
    [SerializeField] private int spawnCost;
    [SerializeField] private int difficultyLevel;
    [SerializeField] private string enemyType;


    public int Cost => spawnCost;
    public int Tier => difficultyLevel;
    public string EnemyType => enemyType;


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
            //xp = Instantiate(xp, gameObject.transform.position, Quaternion.identity); //Create an XP GameObject and make it Addressable
            //xp.GetComponent<XP>().Amount = UnityEngine.Random.Range(minXP, maxXP);  //Edit the XP amount of the Created XP object instance
            Destroy(gameObject);
        } 
    }

    // When the enemy is destroyed, unregister it from the EnemyTracker
    private void OnDestroy()
    {
        // Check if the EnemyTracker instance exists before calling UnregisterEnemy
        if (EnemyTracker.Instance != null)
        {
            // Unregister the enemy from the EnemyTracker
            EnemyTracker.Instance.UnregisterEnemy();
        }
    }
}
