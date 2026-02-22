using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; //the bullet you shoot as a prefab
    [field:SerializeField]public Transform ShootingPoint{get;set;} //the point you shoot from
    [field: SerializeField] public GameObject ShootingMiddle{get;set;} //a point in the middle of the character, to rotate the shooting point around the player
    //also this is a GameObject and not just the transform because it gets buggy with just the transform (idk why tho)

    [field: SerializeField] public float FireRate{get;set;} //your FireRate in shots per second 
    // //the cooldown until you can shoot again --> 1/FireRate
       
    
    [field: SerializeField] public int ShotSpeed{get;set;}


 
    [field: SerializeField] public int Damage{get;set;}

    [field: SerializeField] public int BulletAmount{get;set;}

    public bool CanShoot{get;set;}

    private void Awake()
    {
        CanShoot = true;
    }


    private void Update() //we check for the shooting in Update because we need to register clicks (depends on frames)
    {
        if (CanShoot && Input.GetMouseButton(0))
        {
            Shoot(BulletAmount);
        }
    }


    public void Shoot(int bulletCount) //we need to specify how many Bullets we shoot
    {
        for (int i = 1; i <= bulletCount; i++) //for every bullet fired
        {
            GameObject newBullet = Instantiate(bulletPrefab, ShootingPoint.position, ShootingPoint.rotation); //instantiate new bullet from the Prefab, and set its position and rotation
            newBullet.GetComponent<Rigidbody2D>().AddForce(ShootingPoint.up * ShotSpeed, ForceMode2D.Impulse); //add the velocity in the direction it should go


            //THIS IS NECESSARY IF WE WANT TO SHOOT MORE THAN 1 BULLET
            if (i % 2 == 0) //every other bullet
            {
                ShootingMiddle.transform.Rotate(new Vector3(0, 0, 5 * i)); //goes in one direction
            }
            else
            {

                ShootingMiddle.transform.Rotate(new Vector3(0, 0, -5 * i)); //goes in the other direction
            }
            //we just rotate left and right from the initial point, so we get a bit of a "Spread" although it is not random spread but set spread
        }
        ShootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //reset the rotation in the end so the rotation isnt all messed up next time we want to shoot a bullet
        //we also use "ShootingMiddle" and not just the PlayerObject because we dont want to reset the players rotation
        //that would look very choppy
        StartCoroutine(StartShootCooldown());
    }


    public IEnumerator StartShootCooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(1f / FireRate);
        CanShoot = true;
    }

}