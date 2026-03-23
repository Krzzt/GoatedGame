using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{


    public override void Shoot(int bulletCount)
    {
        
        for (int i = 1; i <= bulletCount; i++) //for every bullet fired
        {
            bulletsLeft--;
            GameObject newBullet = Instantiate(bulletPrefab, ShootingPoint.position, ShootingPoint.rotation); //instantiate new bullet from the Prefab, and set its position and rotation
            newBullet.GetComponent<Rigidbody2D>().AddForce(ShootingPoint.up * ShotSpeed, ForceMode2D.Impulse); //add the velocity in the direction it should go


            //THIS IS NECESSARY IF WE WANT TO SHOOT MORE THAN 1 BULLET
            if (i % 2 == 0) //every other bullet
            {
                ShootingMiddle.transform.Rotate(new Vector3(0, 0, spreadAngle * i)); //goes in one direction
            }
            else
            {

                ShootingMiddle.transform.Rotate(new Vector3(0, 0, -spreadAngle * i)); //goes in the other direction
            }
            //we just rotate left and right from the initial point, so we get a bit of a "Spread" although it is not random spread but set spread
        }
        ShootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //reset the rotation in the end so the rotation isnt all messed up next time we want to shoot a bullet
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

