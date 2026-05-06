using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{


    public override void Shoot(int bulletCount)
    {
        
        for (int i = 1; i <= bulletCount; i++) //for every bullet fired
        {
            ShootingMiddle.transform.Rotate(0,0,Random.Range(-SpreadAngle,SpreadAngle+1));
            bulletsLeft--;
            GameObject newBullet = Instantiate(bulletPrefab, ShootingPoint.position, ShootingPoint.rotation); //instantiate new bullet from the Prefab, and set its position and rotation
            newBullet.GetComponent<Rigidbody2D>().AddForce(ShootingPoint.up * ShotSpeed, ForceMode2D.Impulse); //add the velocity in the direction it should go
            ShootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //reset the rotation in the end so the rotation isnt all messed up next time we want to shoot a bullet
        }
        //we also use "ShootingMiddle" and not just the PlayerObject because we dont want to reset the players rotation
        //that would look very choppy
        if (bulletsLeft > 0)
        {
            StartCoroutine(StartShotDelayCooldown());
        }
        else
        {
            StartCoroutine(StartReloadCooldown()); 
        }

    }
}

