using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    [SerializeField] private int GamePauseManager;

    // Tracks pause state
    private bool isPaused = false;
    
    // Toggle pause state
    public void ChangeGamePauseState()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

}
