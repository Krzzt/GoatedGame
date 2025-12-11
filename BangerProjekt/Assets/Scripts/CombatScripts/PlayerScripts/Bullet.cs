using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float timeAlive; //the max time alive before spontaneously imploding

    public int remainingPierce; //the pierce this bullet still has left. We probably want to get this from the Weapon Script later down the line



    private void Awake()
    {
        timeAlive = 20;
    }

    private void Start()
    {
        //THIS IS FOR TIMEALIVE AND DESTRUCTION
        StartCoroutine(BulletCountDown());
    }
    public IEnumerator BulletCountDown()
    {
        yield return new WaitForSeconds(timeAlive); //wait for the specified time
        Destroy(gameObject); //Destroy the Object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        if (currObject.tag == "Enemy") //if its an enemy (as set by its tag)
        {

            //Damage the Enemy
            remainingPierce--; //reduce the pierce
            if (remainingPierce <= 0) //and if there is no pierce left
            {
                Destroy(gameObject); //and destroy the bullet
            }

        }
        if (currObject.tag == "Wall") //if the bullets collide with a wall
        {
            Destroy(gameObject); //destroy the bullet
        }
    }

}
