using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private float distanceForShooting;
    [SerializeField] private int shotSpeed;


    [field:SerializeField]public int BulletDamage{get;set;}

    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private float fireRate;

    [SerializeField] private int bulletAmount;

    private bool canShoot = true;

    void FixedUpdate()
    {
        TurnToPlayer();
        if (Distance > distanceForShooting) //if the enemy is further away than he should be for shooting
        {
            MoveToPlayer();
        }
        else //if the enemy is close enough
        {
            if (canShoot) //do not put this in a "else if" above, unless you also want to specify that the distance has to be lower than the if above
            {
                Shoot(bulletAmount);
            }

        }
    }


    private void Shoot(int amount)
    {
        for(int i = 0; i < amount ; i++)
        {
            GameObject newBullet = Instantiate(enemyBulletPrefab,shootingPoint.position, shootingPoint.rotation, gameObject.transform);
            //all these parameters: we instantiate the bullethePrefab at the position of the shooting point with the rotation of the enemy
            //the parent of the bullet in the hierarchy will be this gameObject
            //after getting all the values it needs, the bullet will not be the child of this gameobject anymore
            //since we dont want the rotation of the gameobject to transfer over to the bullets
            newBullet.transform.Rotate(new Vector3(0,0,UnityEngine.Random.Range(-10,11)));
            //should the enemy shoot more than 1 bullet, we get that "Shotgun Spread"
            newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.up * shotSpeed, ForceMode2D.Impulse);
        }

        StartCoroutine(ShootingCooldown());
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
    }
}