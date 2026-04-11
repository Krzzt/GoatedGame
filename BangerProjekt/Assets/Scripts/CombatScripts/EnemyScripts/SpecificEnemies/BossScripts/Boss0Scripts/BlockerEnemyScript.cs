using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.VisualScripting;

public class BlockerEnemyScript : Enemy
{
    private GameObject parentBoss;

    new void Awake()
    {
        base.Awake();
        parentBoss = GameObject.FindObjectOfType<Boss>().gameObject; //we find the boss
    }
    public override void DamageUnit(int amount, float crit)
    {
        //it does not give a fuck
    }

    new void TurnToPlayer()
    {
        Vector3 goalPosition = playerObject.transform.position - parentBoss.transform.position;
        goalPosition = parentBoss.transform.position + (0.5f * goalPosition);
        if (!NavMesh.CalculatePath(transform.position, goalPosition, NavMesh.AllAreas, pathToPlayer)) //if we cant get between player and parent
        {
            if(!NavMesh.CalculatePath(transform.position, parentBoss.transform.position, NavMesh.AllAreas, pathToPlayer)) //check if we can get to the parent directly
            {
                //if none of those two work, we are fucked (should always work because parent and blocker are in the same room)
                Debug.LogError("No path to anywhere found. You are fucked");
            }
        }
        nextMovePoint = pathToPlayer.corners.ToList<Vector3>(); //we convert that shit into a list
        Distance = Vector3.Distance(transform.position, playerObject.transform.position);
        direction = nextMovePoint[1] - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0,0,-90);
        //then we turn to where we need to go
        //maybe also needs to get changed when we add sprites later but idfk
    }
}
