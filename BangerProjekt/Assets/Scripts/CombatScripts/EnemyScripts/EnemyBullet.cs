using System.Collections;
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
        if (collision.gameObject.CompareTag("Player")||collision.CompareTag("Obstacle"))
        {
            if(collision.gameObject.GetComponent<Unit>())
            collision.gameObject.GetComponent<Unit>().DamageUnit(damage, 1);
            Destroy(gameObject); //no Pierce or stuff like that necessary since there is only 1 player
            //so why add pierce 
            //maybe some bosses or something want piercing bullets that bounce?
        }
        else if (collision.gameObject.CompareTag("Wall")) //if the bullet hits a wall
        {
            Destroy(gameObject); //Kill the bitch
        }
    }

    public IEnumerator KillBulletTimer()
    {
        yield return new WaitForSeconds(secondsToLive); //Time's running out my dear Bullet
        Destroy(gameObject);
    }
}
