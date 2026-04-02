using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float timeAlive; //the max time alive before spontaneously imploding
    protected Weapon weaponScript;
    protected Player playerScript;
    public Rigidbody2D rb;
    public Vector2 pos;
    public Vector2 poscol;

    private void Awake()
    {
        timeAlive = 20;
        weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        pos = gameObject.transform.position;
    }

    protected virtual void Start()
    {
        //THIS IS FOR TIMEALIVE AND DESTRUCTION
        StartCoroutine(BulletCountDown());
        print(pos);
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
            float  CritValue = 1 + weaponScript.CritDamage / 100f; 
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
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        if (currObject.CompareTag("Enemy")) //if its an enemy (as set by its tag)
        {

            //Damage the Enemy
            float CritDamage = CritCalculate();
            currObject.GetComponent<Enemy>().TakeDamage((int)((weaponScript.Damage * weaponScript.DamageMult) * CritDamage), CritDamage);
            LifeStealCalculate();
            weaponScript.BulletPierce--; //reduce the pierce
            if (weaponScript.BulletPierce <= 0) //and if there is no pierce left
            {
                currObject.GetComponent<Unit>().DamageUnit((int)(weaponScript.Damage * weaponScript.DamageMult));
                RemainingPierce--; //reduce the pierce
                if (RemainingPierce <= 0) //and if there is no pierce left
                {
                    Destroy(gameObject); //and destroy the bullet
                }
            }
            if (weaponScript.BulletBounces > 0)
            {
                weaponScript.BulletBounces--;
            }
        }
        else if (currObject.CompareTag("Wall") || currObject.CompareTag("Door")) //if the bullets collide with a wall
        {
            if (weaponScript.BulletBounces <= 0)
            {
                Destroy(gameObject); //destroy the bullet
            }
            poscol = currObject.transform.position;
            print(poscol);
            Vector2 newVelocity = Vector2.Reflect(pos, poscol.normalized);
            print(newVelocity);
            gameObject.transform.position *= newVelocity;
            print(gameObject.transform.position);
            print(pos);
            
        }
    }

}
