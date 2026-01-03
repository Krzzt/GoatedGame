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
}
