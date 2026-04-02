using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Weapon : MonoBehaviour
{
    protected GameObject bulletPrefab; //the bullet you shoot as a prefab
    public Transform ShootingPoint{get;set;} //the point you shoot from
    public GameObject ShootingMiddle{get;set;} //a point in the middle of the character, to rotate the shooting point around the player
    //also this is a GameObject and not just the transform because it gets buggy with just the transform (idk why tho)

    [field: SerializeField] public float FireRate{get;set;} //your FireRate in shots per second 
    // //the cooldown until you can shoot again --> 1/FireRate
       
    
    [field: SerializeField] public int ShotSpeed{get;set;}


 
    [field: SerializeField] public int Damage{get;set;}
    public float DamageMult { get; set; } = 1f;
    [field: SerializeField] public float LifeSteal{get;set;} // dont mind me nibbling on your neck
    [field: SerializeField] public int BulletBounces{get;set;}
    [field: SerializeField] public int BulletAmount{get;set;}
    public int BulletPierce{get;set;} //the pierce this bullet still has left
    protected int bulletsLeft; //the amount in your magazine

    public bool CanShoot{get;set;}

    [SerializeField] protected int spreadAngle;
    //if you shoot more than 1 bullet at a time (like at the same time), this decides how high the spread for a bullet is (its not random but exact)
    //if this is e.g set to 3 and you fire 4 bullets, the first one goes straight, the second one 3 degrees to the right, the third one 3 degrees to the left
    //and the fourth one 6 degrees to the right etc.
    [SerializeField] protected float shotDelay; //if BulletAmount is higher than 1, this variable is important
    //also important to note that the player can increase their bulletAmount by other means than weapon choice (like Cards, Items etc.)
    //if this is set to 0, every shot is shot at the same time (with spread decided by the spreadAngle)
    //if this is set to e.g 0.1, the delay between the shots in a "magazine" (u have infinite ammo but just need to reload like with a revolver)
    //the shots will come with small cooldown of 0.1. After that, the FireRate or as it is now called "reloadSpeed" comes into play to reload your new bulletAmount
    //so this acts as a kind of "second cooldown" for some weapons that want to use a magazine mechanic
    
    [field:SerializeField] public float CritChance{get;set;} // crit chance
    [field:SerializeField] public float CritDamage{get;set;} // crit Damage

    [field:SerializeField] public WeaponItem CorrespondingItem {get; set;}
    private void Awake()
    {
        CanShoot = true;
        bulletsLeft = BulletAmount;
        ShootingMiddle = GameObject.Find("ShootingMiddle"); //we find by name to not bloat the tags aaaaaaaa help names are so bad aaaaaa
        ShootingPoint = ShootingMiddle.transform.GetChild(0);
        SetItemStats();
    }


    private void Update() //we check for the shooting in Update because we need to register clicks (depends on frames)
    {
        if (CanShoot && Input.GetMouseButton(0))
        {
            if (shotDelay > 0)
            {
                Shoot(1); //one bullet at a time (maybe needs to be changed later)
            }
            else
            {
                Shoot(BulletAmount);   
            }

        }
        //could maybe be improved (performance wise) if we get a function that only triggers onMouseDown
    }



    public abstract void Shoot(int bulletCount); //we need to specify how many Bullets we shoot

    public void SetItemStats()
    {
        bulletPrefab = CorrespondingItem.BulletPrefab;
        Damage += CorrespondingItem.Damage;
        FireRate += CorrespondingItem.FireRate;
        ShotSpeed += CorrespondingItem.ShotSpeed;
        BulletAmount += CorrespondingItem.BulletAmount;
        spreadAngle += CorrespondingItem.SpreadAngle;
        shotDelay += CorrespondingItem.ShotDelayOrRange;
        bulletsLeft = BulletAmount;
        CritDamage += CorrespondingItem.CritDamage;
        CritChance += CorrespondingItem.CritChance;
        LifeSteal += CorrespondingItem.LifeSteal;
        //we add everywhere in case the player shit gets called first to add the fucking stats (i dont think it could happen and even if, the stats would affect the old weapon, but fuck it)
        BulletBounces += CorrespondingItem.BulletBounces;
    }
    

    public IEnumerator StartShotDelayCooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(1f / shotDelay);
        CanShoot = true;
    }

    public IEnumerator StartReloadCooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(1f / FireRate);
        bulletsLeft = BulletAmount;
        CanShoot = true;
    }

}
