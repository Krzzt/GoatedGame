using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MeleeWeapon : Weapon
{
    protected bool isSwinging = false;
    protected int swingNumber = 0;
    protected GameObject currBlade;
    public override void Shoot(int bulletCount)
    {
        CanShoot = false;
        currBlade = Instantiate(bulletPrefab,ShootingPoint.position, ShootingPoint.rotation, ShootingPoint.transform);
        ShootingMiddle.transform.Rotate(0,0,spreadAngle); //set the rotation to the right
        //as a child of the shooting point so it rotates with the shooting point

        InvokeRepeating("CheckForSwing", 0, 0.05f); //check pretty frequently


    }

    public void CheckForSwing()
    {
        float anglePerTick = (2 * spreadAngle) / ((1f / ShotSpeed) * 50f); // 1 / shotDelay = time for it to swing entirely, and we need 2x spreadAngle because we travel from one end to another
        if (!isSwinging)
        {
            if (bulletsLeft >= 0)
            {
                StartCoroutine(SwingWeapon(swingNumber % 2 == 1, anglePerTick)); //so rightToLeft if swingnumber % 2 == 0 and LeftToRight if swingnumber % 2 == 1
                bulletsLeft--;
                swingNumber++;
            }
            else 
            {
                CancelInvoke("CheckForSwing"); //if we are done, we dont check anymore
                ShootingMiddle.transform.rotation = new Quaternion(0, 0, 0, 0); //set back to standard rotation
                StartCoroutine(StartReloadCooldown());
                Destroy(currBlade); //we are done so go away blade
                currBlade = null;
                swingNumber = 0;
            }
        }
    }

    public IEnumerator SwingWeapon(bool rightToLeft, float anglePerTick)
    {
        isSwinging = true;
        float tickAmount = 2 * (spreadAngle / anglePerTick);
        if (!rightToLeft)
        {
            anglePerTick *= -1; //we change direction if we want to go from left to right
        }
        while (tickAmount > 0) //i hate floats but we dont know if it actually matches
        {
            yield return new WaitForFixedUpdate();
            if (tickAmount >= 1)
            {
                ShootingMiddle.transform.Rotate(0, 0, anglePerTick);
                tickAmount--;
            }
            else
            {
                ShootingMiddle.transform.Rotate(0, 0, anglePerTick * tickAmount);
                tickAmount = 0;
            }
        }
        isSwinging = false;
    }
}
