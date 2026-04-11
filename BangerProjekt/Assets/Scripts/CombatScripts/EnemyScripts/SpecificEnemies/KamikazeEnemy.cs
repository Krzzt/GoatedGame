using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy
{
    //Damage for this enemy decides contact damage (which will almost never happen) and explosion damage
    [SerializeField] private float distanceForBomboclat; //how far the enemy has to be for the explosion to start

    [SerializeField] private float timeUntilBomboclat; //the time the player has to react until the enemy actually explodes

    private GameObject rangeIndicatorObject; //this is the grey circle that shows the range of the explosion
    private GameObject redIndicator; //this is he growing red circle

    private float indicatorGrowPerTick;
    [SerializeField] private float bomboClatSize;

    private bool isBomboclating = false;

    new void Awake()
    {
        base.Awake();
        rangeIndicatorObject = gameObject.transform.GetChild(0).gameObject;
        redIndicator = gameObject.transform.GetChild(1).gameObject;
        rangeIndicatorObject.SetActive(false);
        redIndicator.SetActive(false);
    }

    new void Start()
    {
        base.Start();
        InvokeRepeating("CheckForBomboclat",0,0.2f);
    }

    public void CheckForBomboclat()
    {
        if (Distance <= distanceForBomboclat)
        {
            StartBomboclat();
        }
    }

    public void GrowBomboclat()
    {
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
            indicatorGrowPerTick = bomboClatSize / (timeUntilBomboclat * 10f); //*10 since 10 ticks to grow per sec
            StartCoroutine(WaitForBomboclat());
            MoveSpeed = 1.25f;
            CancelInvoke("TurnToPlayer");
            InvokeRepeating("GrowBomboclat",0,0.1f);
        }
    }

    public void Bomboclat()
    {
        //Explode
        if (gameObject.transform.GetChild(0).GetComponent<Collider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<Collider2D>()))
        {
            //if Player and our Collider is Touching, big ouch
            playerScript.DamageUnit(Damage, 1);
        }
        enemyDies?.Invoke(gameObject); //counts As Dying
        Destroy(gameObject);
    }

    public IEnumerator WaitForBomboclat()
    {
        yield return new WaitForSeconds(timeUntilBomboclat);
        Bomboclat();

    }

}
