using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    
    // Singleton instance extra for easy access
    public static EnemyTracker Instance { get; private set; }

    [SerializeField] private WaveController waveController;

    // Track active enemies
    public int ActiveEnemiesCount { get; private set; } = 0;
    
    // Ensure singleton pattern
    private void Awake()
    {
        if (Instance != null)
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object
        Instance = this;
    }

    // Methods to register and unregister enemies
    public void RegisterEnemy()
    {
        // Increment the count of active enemies
        ActiveEnemiesCount++;
        Debug.Log($"Enemy gespawned. Active enemies: {ActiveEnemiesCount}");

    }
    
    // Method to unregister an enemy when it is defeated
    public void UnregisterEnemy()
    {
        // Decrement the count of active enemies
        ActiveEnemiesCount--;
        Debug.Log($"Enemy defeated. Active enemies: {ActiveEnemiesCount}");

        if (ActiveEnemiesCount <= 0)
        {
            Debug.Log("All enemies defeated. Wave complete!");
           /* 
            if (waveController != null)
            {
                waveController.OnWaveFinished();
            }
            */
        }
    }

    public int GetActiveEnemiesCount()
    {
        // Return the current count of active enemies
        return ActiveEnemiesCount;
    }

    
}
