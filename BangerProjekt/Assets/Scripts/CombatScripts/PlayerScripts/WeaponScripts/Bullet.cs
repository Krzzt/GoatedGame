using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Unity.VisualScripting;
public class PlayerBullet : MonoBehaviour
{
    private float timeAlive; //the max time alive before spontaneously imploding
    protected Weapon weaponScript;
    protected Player playerScript;
    private Rigidbody2D rb;
    private int RemainingPierce;
    private int RemainingBulletBounces;
    private Vector2 bulletPos;
    private bool isBouncing;

    private void Awake()
    {
        timeAlive = 20;
        weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        RemainingBulletBounces = weaponScript.BulletBounces;
        RemainingPierce = weaponScript.BulletPierce;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        //THIS IS FOR TIMEALIVE AND DESTRUCTION
        StartCoroutine(BulletCountDown());
    }

    public IEnumerator BulletCountDown()
    {
        yield return new WaitForSeconds(timeAlive); //wait for the specified time
        Destroy(gameObject); //Destroy the Object
    }

    public float CritCalculate() // starts the Crit roulet
    {
        int temp = Random.Range(1, 101);
        if (temp <= weaponScript.CritChance)
        {
            float CritValue = 1 + weaponScript.CritDamage / 100f;
            return CritValue; // returns the crit damage as a 1.x multiplier
        }
        else return 1; //1 means a multiplier of 1.0, so normal DMG
    }

    public void LifeStealCalculate() // starts the lifesteal gambling
    {
        int temp = Random.Range(1, 101);
        if (temp <= weaponScript.LifeSteal)
        {
            playerScript.ApplyLifesteal(); // sends the success of hitting the lifsteal to the player
        }
        else return;
    }

    public void BounceBullet(GameObject currObject)
    { // Like a smart man ones said "Einfallswinkel = Ausfallswinkel" but i never thought he meant some bs like this
        StartCoroutine(SecureOneBouncePerFrame());

        Vector2 bulletvelocity = rb.velocity; // getting the Velocity 
        Vector2 closestPoint = currObject.GetComponent<Collider2D>().ClosestPoint(bulletPos); // getting the exact collison point
        Vector2 normal = (bulletPos - closestPoint).normalized; // getting a normalized vector of our collsion 

        RemainingBulletBounces--; // now we remove a Bounce

        rb.velocity = Vector2.Reflect(bulletvelocity, normal); // calculating the new velocity, with the Reflect function (on the normal)
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f; // Atan2 converts the velocity to an Angle in degrees // -90f because how the sprite is drawn
        transform.rotation = Quaternion.Euler(0f, 0f, angle); // Here i apply the Angle to the Rotation Axis (Z) 
    }

    public IEnumerator SecureOneBouncePerFrame() // no more bouncing thru walls
    {
        yield return new WaitForEndOfFrame();
        isBouncing = false; 
    }

    public void BounceInitiator(GameObject currObject)
    {
        if (isBouncing) return;
        if (RemainingBulletBounces > 0)
        {
            isBouncing = true;
            bulletPos = transform.position; // getting current bullet position

            BounceBullet(currObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckObstacleAndSetBehaivour(GameObject currObject)
    {
        if (currObject.GetComponent<ObstacleScript>().Obstacle.Passable) return; //we dont care about passable obstacles
        if (currObject.GetComponent<DestroyableObstacle>()) //we damage obstacles that u can destroy
        {
            DamageCalculation(currObject, false);
        }
        else{ BounceInitiator(currObject); } //else means that they are basically like walls, so we bounce
    }

    public void DamageCalculation(GameObject currObject, bool isLifestealable)
    {
        if (isLifestealable) LifeStealCalculate(); 
        float CritDamage = CritCalculate();
        currObject.GetComponent<Unit>().DamageUnit((int)((weaponScript.Damage * weaponScript.DamageMult) * CritDamage), CritDamage);
        RemainingPierce--; //reduce the pierce
        if (RemainingPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        switch (currObject.tag)
        {
            case "Enemy":DamageCalculation(currObject, true); break; //we just damage enemies, and we can lifesteal from them
            case "Wall":BounceInitiator(currObject); break; //bounce on walls
            case "Door":BounceInitiator(currObject); break; //bounce on doors
            case "Obstacle":CheckObstacleAndSetBehaivour(currObject); break; //check which type of obstacle and do stuff accordingly
            default: ; break;
        }
        
    }
} 
