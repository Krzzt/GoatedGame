using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] private float distanceForDashing; //distance needed until a dash can start (length of the vector)
    [SerializeField] private float dashCooldown;
    private bool dashReady;

    [SerializeField] private float dashDuration;

    [SerializeField] private float dashSpeedIncrease; //by how much the speed increases (1 = 100% speed, 5 = 500% speed)

    new void Awake()
    {
        base.Awake();
        dashReady = true;
    }

    new void Start()
    {
        base.Start();
        InvokeRepeating("CheckForDash",0,0.2f);
    }
    void CheckForDash()
    {
        if (dashReady && Distance <= distanceForDashing) 
        {
            Dash();
        }
    }

    public void Dash()
    {
        dashReady = false;
        MoveSpeed *= dashSpeedIncrease; //increase the dash speed
        CancelInvoke("TurnToPlayer");
        //start 2 coroutines: 1 to check the dash cooldown and the other to check the dash duration
        StartCoroutine(WaitForDashReady());
        StartCoroutine(WaitForEndDash());

    }

    public IEnumerator WaitForDashReady()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashReady = true;
    }

        public IEnumerator WaitForEndDash()
    {
        yield return new WaitForSeconds(dashDuration);
        MoveSpeed /= dashSpeedIncrease;
        InvokeRepeating("TurnToPlayer",0,0.2f);
    }
}
