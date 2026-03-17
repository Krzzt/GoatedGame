using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy //omg its abstract
{
    [SerializeField] protected float attackCooldown; //how long should we wait until the next attack happens
    //beware: The cooldown starts immediately, after an attack started, not after it ended D:
    [SerializeField] protected int attackAmount; //how many different attacks does the boss have

    protected new void Start()
    {
        InvokeRepeating("TurnToPlayer",0,0.2f); //no moving to the player just turning and calculating a path
        StartCoroutine(WaitForNextAttack());
    }

    protected abstract void Attack();
    protected IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        Attack();
    }

}
