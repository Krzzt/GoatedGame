using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    // ========== Enemy Pool ==========
    [SerializeField] private List<EnemyList> enemyPool;

    // ========== References ==========
    [SerializeField] private Transform playerTransform;

    [SerializeField] private SpawnPositionProvider spawnPositionProvider;
    [SerializeField] private SpawnValidator spawnValidator;

    // ========== Wave Settings ==========
    [SerializeField] private int waveBudget = 10;

    // ========== Spawn Distance ==========
    [SerializeField] private float minSpawnDistance = 6f;
    [SerializeField] private float maxSpawnDistance = 12f;

    // ========== Debug ==========
    [SerializeField] private bool drawDebugGizmos = true;



    public void SpawnOneEnemy()
    {

        // Validate enemy pool and references
        if (enemyPool == null || enemyPool.Count == 0)
        {
            Debug.LogError("EnemySpawner: Enemy pool is empty or not assigned!");
            return;
        }

        if (spawnPositionProvider == null || spawnValidator == null)
        {
            Debug.LogError("EnemySpawner: SpawnPositionProvider reference is missing!");
            return;
        }

        // Alternative simpler spawn logic without validation   
        EnemyList enemyToSpawn = enemyPool[0];

        // Get spawn position
        Vector2 spawnPos = spawnPositionProvider.GetSpawnPosition();

        // Validate spawn position
        if (!spawnValidator.IsSpawnPositionValid(spawnPos))
        {
            Debug.Log("EnemySpawner: Invalid spawn position, enemy not spawned.");
            return;
        }

        // Spawn the enemy
        Instantiate(enemyToSpawn.enemyPrefab, spawnPos, Quaternion.identity);
    }

    
}
