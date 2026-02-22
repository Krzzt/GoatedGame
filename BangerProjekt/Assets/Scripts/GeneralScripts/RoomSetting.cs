using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
    public class BossWave
    {
        public int waveNumber; // The wave number at which the boss appears
        public GameObject bossPrefab; // The boss prefab to spawn
    }


public class RoomSetting : MonoBehaviour
{
    [Header("Room Settings")]

    [SerializeField] private int wavesPerRoom = 3;  // Number of waves in each room
    [SerializeField] private int baseWaveBudget = 10; // Base budget for the first wave
    [SerializeField] private int budgetMultiplier = 2; // Multiplier for budget increase per wave
    [SerializeField] private int maxEnemyTier = 1; // Maximum enemy tier allowed in this room


    [Header("Boss Waves")]
    [SerializeField] private List<BossWave> bossWaves; // List of boss waves in this room


    [Header("Tags / Filters")]
    [SerializeField] private List<string> allowedEnemyTags; // Tags of enemies allowed in this room

    
    // Public getters for private fields
    public int WavesPerRoom => wavesPerRoom;
    public int BaseWaveBudget => baseWaveBudget;
    public int BudgetMultiplier => budgetMultiplier;
    public int MaxEnemyTier => maxEnemyTier;
    
    public List<BossWave> BossWaves => bossWaves;

    public List<string> AllowedEnemyTags => allowedEnemyTags;
}
