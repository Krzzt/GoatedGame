using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private float distanceForShooting;
    [SerializeField] private int shotSpeed;

    [SerializeField] private int bulletDamage;
    public int BulletDamage
    {
        get
        {
            return bulletDamage;
        }
        set
        {
            bulletDamage = value;
        }
    }

    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private float fireRate;

    private bool canShoot = true;

    void FixedUpdate()
    {
        TurnToPlayer();
        if (Distance > distanceForShooting) //if the enemy is further away than he should be for shooting
        {
            MoveToPlayer();
            Debug.Log("Moving to Player");
        }
        else //if the enemy is close enough
        {
            Debug.Log("Not shooting yet");
            if (canShoot) //do not put this in a "else if" above, unless you also want to specify that the distance has to be lower than the if above
            {
                Debug.Log("SHOOT");
                Shoot();
            }

        }
    }


    private void Shoot()
    {
        GameObject newBullet = Instantiate(enemyBulletPrefab,shootingPoint.position, shootingPoint.rotation, gameObject.transform);
        //all these parameters: we instantiate the bullethePrefab at the position of the shooting point with the rotation of the enemy
        //the parent of the bullet in the hierarchy will be this gameObject
        //after getting all the values it needs, the bullet will not be the child of this gameobject anymore
        //since we dont want the rotation of the gameobject to transfer over to the bullets
        newBullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.up * shotSpeed, ForceMode2D.Impulse);
        StartCoroutine(ShootingCooldown());
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
    }



}
