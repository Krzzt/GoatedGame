using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int startWave = 0;
    [SerializeField] private int baseBudget = 5;
    [SerializeField] private float budgetIncreasePerWave = 5f;

    [Header("References")]
    [SerializeField] private EnemySpawner enemySpawner;

    private int currentWave;

    private void Start()
    {
        currentWave = startWave;
        
    }

    public void StartNextWave()
    {
        int waveBudget = CalculateBudgetForWave(currentWave);

        Debug.Log($"wave {currentWave} starting with budget {waveBudget}");

        enemySpawner.SpawnEnemiesWithBudget(waveBudget);

        currentWave++;
    }

    public int GetCurrentWave()
    {
         return currentWave;
    }

    private int CalculateBudgetForWave(int wave)
    {
        return baseBudget + (int)((wave - 1) * budgetIncreasePerWave);
    }


    public void OnWaveFinished()
    {
        Debug.Log("WaveController: Wave finished. Preparing for next wave...");
        // Later:
        // UI 
        // Break time
        // Loot??
        // Boss checks
        // Room change
        // etc.....

        // For now, just start the next wave immediately
        StartNextWave();
    }



    // Missing.... not implemented jet
    //UI...next
    //Timer
    //Boss-Logic

}
