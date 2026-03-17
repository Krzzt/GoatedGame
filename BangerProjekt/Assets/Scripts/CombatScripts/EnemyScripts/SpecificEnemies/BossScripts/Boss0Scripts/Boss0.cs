using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Boss0 : Boss
{
    [SerializeField] private GameObject blockerPrefab;
    [SerializeField] private int blockerAmount;
    private int currentBlockerAmount = 0;
    [SerializeField] private GameObject rangedPrefab;
    [SerializeField] private int rangedAmount;
    private int currentRangedAmount = 0;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private int meleeAmount;
    private int currentMeleeAmount = 0;

    private RoomScript currRoom;


    new void Start()
    {
        currRoom = GameManager.currentRoom.GetComponent<RoomScript>();
        InvokeRepeating("MoveAway",0,1f); //gets a new point to move away to every second
        StartCoroutine(WaitForNextAttack());
    }
    protected override void Attack()
    {
        int minionChosen = Random.Range(0,3); //we want to spawn a random minion
        GameObject newMinion = null;
        switch(minionChosen) //after that we check
        {
            case 0:
                if (currentBlockerAmount < blockerAmount) //can we spawn a minion of that kind without overstepping our cap
                {
                     newMinion = Instantiate(blockerPrefab); //if it is possible, we instantiate the new minion
                }
            break;

            case 1:
                if (currentRangedAmount < rangedAmount)
                {
                    newMinion = Instantiate(rangedPrefab);
                }
            break;

            case 2:
                if (currentMeleeAmount < meleeAmount)
                {
                    newMinion = Instantiate(meleePrefab);
                }
            break;

            default: Debug.LogError("Wrong Number! couldnt spawn minion!"); break;
        }
        if (newMinion) //and if a new minion exists (so if it isnt null)
        {
            newMinion.transform.position = currRoom.Spawnpoints[Random.Range(0,currRoom.Spawnpoints.Count)].transform.position;
            //we set a new position randomly in the room
        }

        StartCoroutine(WaitForNextAttack());
    }

    private void MoveAway()
    {
        GameObject furthestPoint = currRoom.Spawnpoints[0];
        float howFar = 0f;
        foreach(GameObject point in currRoom.Spawnpoints)
        {
            float currPointDistance = Vector2.Distance(playerObject.transform.position,point.transform.position);
            if (currPointDistance > howFar)
            {
                furthestPoint = point;
                howFar = currPointDistance;
                //we get the furthest point from the player
            }
        }
        if (NavMesh.CalculatePath(transform.position, furthestPoint.transform.position, NavMesh.AllAreas, pathToPlayer)) //if we find a path
        {
            //we want to calculate our path to this furthest point to go there
            nextMovePoint = pathToPlayer.corners.ToList<Vector3>(); //we convert that shit into a list
        }
        direction = nextMovePoint[1] - transform.position;
        direction.Normalize();
        RB.velocity = direction * MoveSpeed;
        //and then we turn and move there
    }

    public new void Die()
    {
        List<GameObject> allEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach(GameObject enemyObject in allEnemies)
        {
            if (enemyObject != gameObject)
            {
                enemyObject.GetComponent<Enemy>().Die(); //kill every enemy but myself
            }
        }
        base.Die(); //because it dies here :)
    }
}
