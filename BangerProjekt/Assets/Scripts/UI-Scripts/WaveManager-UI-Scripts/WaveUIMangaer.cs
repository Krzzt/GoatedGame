using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TextMeshPro is needed for text elements
using TMPro;


public class WaveMangaer : MonoBehaviour
{
    [SerializeField] private WaveController waveController;
    [SerializeField] private EnemyTracker enemyTracker;

    [Header("UI Elements")]
    [SerializeField] public TextMeshProUGUI waveText;
    [SerializeField] public TextMeshProUGUI enemiesCountText;

    private void Update()
    {
        UpdateWaveText();
        UpdateEnemiesCountText();
    }

    private void UpdateWaveText()
    {
        if (waveText != null && waveController != null)
        {
            waveText.text = $"Wave: {waveController.GetCurrentWave()}";
        }
    }

    private void UpdateEnemiesCountText()
    {
        if (enemiesCountText == null && enemyTracker == null)
            return;

        enemiesCountText.text = $"Enemies: {enemyTracker.GetActiveEnemiesCount()}";
        
    }
}
