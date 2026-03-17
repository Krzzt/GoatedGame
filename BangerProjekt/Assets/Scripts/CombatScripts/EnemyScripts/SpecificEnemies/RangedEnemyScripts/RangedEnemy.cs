using System;
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

    [SerializeField] private int spreadAngle;

    new void Start()
    {
        base.Start();
        InvokeRepeating("Shoot",0,1/fireRate); //fireRate = shots per second so we need 1/fireRate (frequency to time)
    }
    private void Shoot()
    {
        if (Distance <= distanceForShooting)
        {
            for(int i = 0; i < bulletAmount ; i++) //shoot as many bullets as the bulletAmount states
            {
                GameObject newBullet = Instantiate(enemyBulletPrefab,shootingPoint.position, shootingPoint.rotation, gameObject.transform);
                //all these parameters: we instantiate the bulletPrefab at the position of the shooting point with the rotation of the enemy
                //the parent of the bullet in the hierarchy will be this gameObject
                //after getting all the values it needs, the bullet will not be the child of this gameobject anymore
                //since we dont want the rotation of the gameobject to transfer over to the bullets
                newBullet.transform.Rotate(new Vector3(0,0,UnityEngine.Random.Range(-spreadAngle,spreadAngle+1)));
                //should the enemy shoot more than 1 bullet, we get that "Shotgun Spread"
                newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.up * shotSpeed, ForceMode2D.Impulse);
            } 
        }


    }
    public new void MoveToPlayer()
    {
        if (Distance > distanceForShooting)
        {
            base.MoveToPlayer();
        }
        else
        {
            RB.velocity = new Vector2(0,0);
        }
    }
}