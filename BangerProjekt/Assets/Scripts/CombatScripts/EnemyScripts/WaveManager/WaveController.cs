using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int startWave = 1;
    [SerializeField] private int baseBudget = 10;
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

    private int CalculateBudgetForWave(int wave)
    {
        return baseBudget + (int)((wave - 1) * budgetIncreasePerWave);
    }



    // Missing.... not implemented jet
    //Wave-Ende-Checks....on it
    //Enemy-Count....on it
    //UI...next
    //Timer
    //Boss-Logic

}
