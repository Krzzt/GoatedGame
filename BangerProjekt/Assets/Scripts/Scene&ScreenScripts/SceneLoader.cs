using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Name der sene die geladen werde soll wenn der knopf gedrukt wird
    //SerializeField macht die private variable im inspector sichtbar
    [SerializeField]public string sceneToLoad="Game";

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene to load is not set!");
            return;
        }

        //Lade die scene mit dem namen der in der variable "sceneToLoad" gespeichert ist
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    
    // Quit the application
    public void QuitGame()
    {
        //Beendet die Anwendung
        Application.Quit();
        Debug.Log("SceneLoader: Game is exiting"); //Nur im Editor sichtbar
    }
   
}
