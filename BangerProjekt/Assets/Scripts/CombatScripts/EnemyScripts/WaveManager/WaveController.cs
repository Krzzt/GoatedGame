using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private RoomSetting currentRoom;

    private int currentWave = 1;
    

    public void StartNextWave()
    {
        int budget = currentRoom.BaseWaveBudget + currentWave * currentRoom.BaseWaveBudgetMultiplier;
        int MaxTier = currentRoom.MaxEnemyTier;

        EnemySpawner.SpawnWave(budget, MaxTier, currentRoom.AllowedEnemyTags);


        if (BossWave.waveNumber == currentWave && BossWave.bossPrefab != null)
        {
            enemySpawner.SpawnBoss(BossWave.bossPrefab)
        }
        
    }




    // Missing.... not implemented jet
    //UI...next
    //Timer
    //Boss-Logic

}
