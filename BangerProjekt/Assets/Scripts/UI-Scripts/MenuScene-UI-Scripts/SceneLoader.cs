//UnityEngine.SceneManagement is it needed to load scenes??
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Name of the scene that should be loaded if the button is pressed
    public string sceneToLoad="Game";

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene to load is not set!");
            return;
        }

        //Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    
    // Quit the application
    public void QuitGame()
    {
        //End the application...
        Application.Quit();
        Debug.Log("SceneLoader: Game is exiting"); //Only visible in the editor
    }
   
}
