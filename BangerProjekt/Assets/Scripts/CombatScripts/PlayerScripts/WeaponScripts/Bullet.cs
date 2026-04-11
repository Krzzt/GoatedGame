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
    public Rigidbody2D rb;
    bool isLifestealable;
    private int RemainingPierce;
    private int RemainingBulletBounces;
    public Vector2 bulletPos;
    public bool isBouncing;

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
        else return 1;
    }
    public void LifeStealCalculate() // starts the lifesteal gambling
    {
        int temp = Random.Range(1, 101);
        if (temp <= weaponScript.LifeSteal)
        {
            playerScript.StealALife(); // sends the success of hitting the lifsteal to the player
        }
        else return;
    }
    public void BulletBounceCalculate(Rigidbody2D _Rigidbody, GameObject currObject)
    { // Like a smart man ones said "Einfallswinkel = Ausfallswinkel" but i never thought he meant some bs like this
        StartCoroutine(IsBouncingcd());

        Vector2 bulletvelocity = _Rigidbody.velocity; // getting the Velocity 
        Vector2 closestPoint = currObject.GetComponent<Collider2D>().ClosestPoint(bulletPos); // getting the exact collison point

        Vector2 normal = (bulletPos - closestPoint).normalized; // getting a normalized vector of our collsion 
        Vector2 newVelocity = Vector2.Reflect(bulletvelocity, normal); // Reflecting the bullet "mirroring" it on the normal

        RemainingBulletBounces--; // now we remove a Bounce

        rb.velocity = newVelocity; // calculating the new velocity
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f; // Atan2 converts the velocity to an Angle in degrees // -90f because how the sprite is drawn
        transform.rotation = Quaternion.Euler(0f, 0f, angle); // Here i apply the Angle to the Rotation Axis (Z) 
    }
    public IEnumerator IsBouncingcd() // no more bouncing thru walls
    {
        yield return new WaitForSeconds(0.001f);//WaitForNextFrameUnit(); //WaitForEndOfFrame();
        isBouncing = false; 
    }
    public void BounceInitiator(GameObject currObject)
    {
        if (isBouncing) return;
        if (!isBouncing && RemainingBulletBounces > 0)
        {
            isBouncing = true;
            bulletPos = transform.position; // getting current bullet position

            BulletBounceCalculate(rb, currObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ObstacleTester(GameObject currObject)
    {
        if (currObject.GetComponent<ObstacleScript>().Obstacle.Passable) { return; }
        if (currObject.GetComponent<DestroyableObstacle>())
        {
            DamageCalculation(currObject, false);
        }
        else{ BounceInitiator(currObject); }
    }
    public void DamageCalculation(GameObject currObject, bool isLifestealable)
    {
        if (isLifestealable) { LifeStealCalculate(); }
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
            case "Enemy":DamageCalculation(currObject, true); break;
            case "Wall":BounceInitiator(currObject); break;
            case "Door":BounceInitiator(currObject); break;
            case "Obstacle":ObstacleTester(currObject); break;
            default: ; break;
        }
        
    }
} 