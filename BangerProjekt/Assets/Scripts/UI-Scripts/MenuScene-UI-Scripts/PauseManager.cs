using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    [SerializeField] private int GamePauseManager;


    private bool isPaused = false;

    public void ChangeGamePauseState()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

}
