using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   private int damage;
   [SerializeField] private int secondsToLive; //implosion and stuff


    void Awake()
    {
        damage = gameObject.transform.GetComponentInParent<RangedEnemy>().BulletDamage;
        gameObject.transform.parent = null;
        //we get the damage of the parent and set a new one
        StartCoroutine(KillBulletTimer());
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject); //no Pierce or stuff like that necessary since there is only 1 player
            //so why add pierce
        }
        else if (collision.gameObject.CompareTag("Wall")) //if the bullet hits a wall
        {
            Destroy(gameObject); //Kill the bitch
        }
    }

    public IEnumerator KillBulletTimer()
    {
        yield return new WaitForSeconds(secondsToLive);
        Destroy(gameObject);
    }
}
