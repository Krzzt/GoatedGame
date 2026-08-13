using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void GiveUp()
    {
        Application.Quit();
    }
}
