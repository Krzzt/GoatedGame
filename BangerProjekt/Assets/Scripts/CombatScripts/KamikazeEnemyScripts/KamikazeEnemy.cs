using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy
{
    //Damage for this enemy decides contact damage (which will almost never happen) and explosion damage
    [SerializeField] private float distanceForBomboclat; //how far the enemy has to be for the explosion to start

    [SerializeField] private float timeUntilBomboclat; //the time the player has to react until the enemy actually explodes

    private GameObject rangeIndicatorObject; //this is the grey circle that shows the range of the explosion
    private rangeIndicatorDamageGiver playerInRangeScript;
    private GameObject redIndicator; //this is he growing red circle

    private float indicatorGrowPerTick;
    [SerializeField] private float bomboClatSize;

    private bool isBomboclating;

    new void Awake()
    {
        base.Awake();
        rangeIndicatorObject = gameObject.transform.GetChild(0).gameObject;
        redIndicator = gameObject.transform.GetChild(1).gameObject;
        rangeIndicatorObject.SetActive(false);
        redIndicator.SetActive(false);

        playerInRangeScript = rangeIndicatorObject.GetComponent<rangeIndicatorDamageGiver>();
    }

    new void FixedUpdate()
    {
       if (!isBomboclating)
        {
            base.FixedUpdate();
        }
        else
        {
            MoveSpeed = 0.25f;
            MoveToPlayer();
        }

        if (Distance <= distanceForBomboclat)
        {
            StartBomboclat();
        }
        if (isBomboclating)
        {
            redIndicator.transform.localScale = new Vector3(redIndicator.transform.localScale.x + indicatorGrowPerTick, redIndicator.transform.localScale.y + indicatorGrowPerTick, 1);
        }
    }

    public void StartBomboclat()
    {
        if (!isBomboclating)
        {
            isBomboclating = true;
            rangeIndicatorObject.SetActive(true); //set the Rangeindicator for the Bomboclat true
            redIndicator.SetActive(true);
            indicatorGrowPerTick = bomboClatSize / (timeUntilBomboclat * 50f);
            StartCoroutine(WaitForBomboclat());
        }
    }

    public void Bomboclat()
    {
        //Explode
        if (playerInRangeScript.PlayerIsInRange)
        {
            playerScript.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

    public IEnumerator WaitForBomboclat()
    {
        yield return new WaitForSeconds(timeUntilBomboclat);
        Bomboclat();

    }

}
