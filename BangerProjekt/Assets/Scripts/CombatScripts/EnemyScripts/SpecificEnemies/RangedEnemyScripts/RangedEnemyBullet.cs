using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBullet : MonoBehaviour
{
   private int damage;
   [SerializeField] private int secondsToLive; //implosion and stuff


    void Awake()
    {
        damage = gameObject.transform.GetComponentInParent<RangedEnemy>().BulletDamage;
        gameObject.transform.parent = GameObject.Find("Game").transform;
        StartCoroutine(killBulletTimer());
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject); //no Pierce or stuff like that necessary since there is only 1 player
            //so why add pierce
        }
    }

    public IEnumerator killBulletTimer()
    {
        yield return new WaitForSeconds(secondsToLive);
        Destroy(gameObject);
    }
}
