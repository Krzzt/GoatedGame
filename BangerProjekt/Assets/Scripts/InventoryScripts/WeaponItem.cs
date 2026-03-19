using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponItem : Item
{
    public float shotDelay;
    public int spreadAngle;
    public int shotSpeed;
    public GameObject bulletPrefab;
}
