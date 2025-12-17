using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPoint; //the point you shoot from
    public GameObject shootingMiddle; //a point in the middle of the character, to rotate the shooting point around the player
    //also this is a GameObject and not just the transform because it gets buggy with just the transform (idk why tho)

    public float fireRate; //your firerate in shots per second
    //private float ShootingCooldown; //the cooldown until you can shoot again --> 1/fireRate
    public bool canShoot;

    public GameObject bulletPrefab; //the bullet you shoot as a prefab
    public int shotSpeed; //the shotspeed (force it gets shot with)

    public int bulletAmount; //amount of bullets you shoot

    private void Awake()
    {
        canShoot = true;
    }


    private void Update() //we check for the shooting in Update because we need to register clicks (depends on frames)
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            Shoot(bulletAmount);
        }
    }


    public void Shoot(int bulletCount) //we need to specify how many Bullets we shoot
    {
        for (int i = 1; i <= bulletCount; i++) //for every bullet fired
        {


            GameObject newBullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation); //instantiate new bullet from the Prefab, and set its position and rotation
            newBullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.up * shotSpeed, ForceMode2D.Impulse); //add the velocity in the direction it should go


            //THIS IS NECESSARY IF WE WANT TO SHOOT MORE THAN 1 BULLET
            if (i % 2 == 0) //every other bullet
            {
                shootingMiddle.transform.Rotate(new Vector3(0, 0, 5 * i)); //goes in one direction
            }
            else
            {

                shootingMiddle.transform.Rotate(new Vector3(0, 0, -5 * i)); //goes in the other direction
            }
            //we just rotate left and right from the initial point, so we get a bit of a "Spread" although it is not random spread but set spread
        }
        shootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //reset the rotation in the end so the rotation isnt all messed up next time we want to shoot a bullet
        //we also use "ShootingMiddle" and not just the PlayerObject because we dont want to reset the players rotation
        //that would look very choppy
        StartCoroutine(StartShootCooldown());
    }


    public IEnumerator StartShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }

}
