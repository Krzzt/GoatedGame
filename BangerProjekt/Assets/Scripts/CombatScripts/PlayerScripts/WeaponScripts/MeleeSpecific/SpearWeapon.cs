using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWeapon : MeleeWeapon
{
    protected Vector3 goalPos;
    protected GameObject goalObject = null;
    public float Range //renaming shotDelay for the spear
    {
        get
        {
            return shotDelay;
        }
        set
        {
            shotDelay = value;
        }
    }
    public override void Shoot(int bulletCount)
    {
        CanShoot = false;
        currBlade = Instantiate(bulletPrefab, ShootingPoint.position, ShootingPoint.rotation, ShootingPoint.transform);
        goalPos =  ShootingMiddle.transform.position + ((ShootingPoint.transform.position - ShootingMiddle.transform.position).normalized * Range);
        GameObject temp = new GameObject(name = "goalPoint");
        goalObject = Instantiate(temp, goalPos, Quaternion.identity, ShootingPoint.transform);
        Destroy(temp);
        InvokeRepeating("CheckForSwing", 0, 0.05f); //check pretty frequently

    }

    public new void CheckForSwing()
    {
        if (!isSwinging)
        {
            if (bulletsLeft > 0)
            {
                StartCoroutine(SwingWeapon()); //so rightToLeft if swingnumber % 2 == 0 and LeftToRight if swingnumber % 2 == 1
                bulletsLeft--;
                swingNumber++;
                Debug.Log("Next Swing!");
            }
            else
            {
                CancelInvoke("CheckForSwing"); //if we are done, we dont check anymore
                ShootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //set back to standard rotation
                StartCoroutine(StartReloadCooldown());
                Destroy(currBlade); //we are done so go away blade
                Destroy(goalObject);
                currBlade = null;
                goalObject = null;
                swingNumber = 0;
            }
        }
    }

    public IEnumerator SwingWeapon()
    {
        isSwinging = true;
        float tickAmount = Range / (ShotSpeed / 5f);
        float ticksDone = 0f;
        while(ticksDone < tickAmount)
        {
            currBlade.GetComponent<Rigidbody2D>().MovePosition(ShootingPoint.transform.position + (goalObject.transform.position - ShootingPoint.transform.position) * (ticksDone / tickAmount));
            yield return new WaitForFixedUpdate();
            ticksDone++;

        }
        ticksDone = 0f;
        while (ticksDone < tickAmount)
        {
            currBlade.GetComponent<Rigidbody2D>().MovePosition(goalObject.transform.position + (ShootingPoint.transform.position - goalObject.transform.position) * (ticksDone / tickAmount));
            yield return new WaitForFixedUpdate();
            ticksDone++;
        }
        isSwinging = false;
    }
}
