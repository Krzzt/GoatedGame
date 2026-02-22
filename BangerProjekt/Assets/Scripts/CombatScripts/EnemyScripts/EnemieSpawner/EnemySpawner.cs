using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyList> allEnemies; // List of all possible enemies to spawn
    public void SpawnWave(int budget, int maxTier, List<string> allowedTags)
    {
        int waveBudget = budget;

        // Filter enemy pool based on max tier and allowed tags
        List<EnemyList> validEnemies = allEnemies.Where(e => e.Tier <= maxTier && (allowedTags == null || allowedTags.Count == 0 || allowedTags.Contains(e.EnemyType))).ToList();

        foreach (var enemy in allEnemies)
        {
            if (enemy.Tier <= maxTier)
            {
                if (allowedTags == null || allowedTags.Count == 0 || allowedTags.Contains(enemy.Type))
                    validEnemies.Add(enemy);
                
            }
        }

        int remainingBudget = budget;

        while (remainingBudget > 0 && validEnemies.Count > 0)
        {
            // Get a list of enemies that can be spawned within the remaining budget
            List<EnemyList> affordableEnemies = validEnemies.FindAll(e => e.Cost <= remainingBudget);

            if (affordableEnemies.Count == 0)
            {
                Debug.Log("No more affordable enemies to spawn.");
                break; // Exit if no enemies can be afforded
            }

            // Randomly select an enemy from the affordable list
            EnemyList selectedEnemy = affordableEnemies[Random.Range(0, affordableEnemies.Count)];

            // Spawn the enemy
            SpawnEnemy(selectedEnemy);

            // Deduct the spawn cost from the remaining budget
            remainingBudget -= selectedEnemy.SpawnCost;
        }


    }


    
}
