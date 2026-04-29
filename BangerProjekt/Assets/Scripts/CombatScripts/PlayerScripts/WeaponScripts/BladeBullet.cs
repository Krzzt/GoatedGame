using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeBullet : PlayerBullet // Blade bullet is now a child of Playerbullet
{
    protected override void Start()
    { 
        // NOTHING END
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        if (currObject.CompareTag("Enemy")||currObject.CompareTag("Obstacle")) //if its an enemy (as set by its tag)
        {
            //Damage the Enemy
            float CritDamage = CritCalculate();
            currObject.GetComponent<Enemy>().DamageUnit((int)((weaponScript.Damage * weaponScript.DamageMult) * CritDamage),CritDamage);
            LifeStealCalculate(); // starts the lifesteal gambling
        }

    }
}
