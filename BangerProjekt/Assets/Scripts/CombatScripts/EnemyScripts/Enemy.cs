using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private float distance;
    public float Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }

    private GameObject playerObject;

    [SerializeField] private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }



    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
        CurrentHealth = MaxHealth;
    }
    void FixedUpdate()
    {
        TurnToPlayer();
        MoveToPlayer();
    }
    public void TurnToPlayer()
    {
        distance = Vector2.Distance(transform.position, playerObject.transform.position);
        Vector2 direction = playerObject.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0,0,-90);
        //turning stuff
        //basically we get the distance between enemy and player but as a Vector and then we turn that vector into an angle
    }
    public void MoveToPlayer()
    {

        transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, MoveSpeed * Time.fixedDeltaTime);
        //math and moving stuff and bla bla bla 
        //no but fr we just use the "Vector2.MoveTowards" function
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
            Destroy(gameObject);
        } 
    }
}
