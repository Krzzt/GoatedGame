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
        InvokeRepeating("MoveAway",0,1f);
        StartCoroutine(WaitForNextAttack());
    }
    protected override void Attack()
    {
        int minionChosen = Random.Range(0,3);
        GameObject newMinion = null;
        switch(minionChosen)
        {
            case 0:
                if (currentBlockerAmount < blockerAmount)
                {
                     newMinion = Instantiate(blockerPrefab);
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
        if (newMinion)
        {
            newMinion.transform.position = currRoom.Spawnpoints[Random.Range(0,currRoom.Spawnpoints.Count)].transform.position;
        }

        StartCoroutine(WaitForNextAttack());
    }

    private void MoveAway()
    {
        GameObject furthestPoint = currRoom.Spawnpoints[0];
        GameObject player = GameObject.FindWithTag("Player");
        float howFar = 0f;
        foreach(GameObject point in currRoom.Spawnpoints)
        {
            float currPointDistance = Vector2.Distance(player.transform.position,point.transform.position);
            if (currPointDistance > howFar)
            {
                furthestPoint = point;
                howFar = currPointDistance;
            }
        }
        if (NavMesh.CalculatePath(transform.position, furthestPoint.transform.position, NavMesh.AllAreas, pathToPlayer)) //if we find a path
        {
            nextMovePoint = pathToPlayer.corners.ToList<Vector3>(); //we convert that shit into a list
        }
        direction = nextMovePoint[1] - transform.position;
        direction.Normalize();
        RB.velocity = direction * MoveSpeed;
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
        base.Die(); //because i die here :)
    }
}
