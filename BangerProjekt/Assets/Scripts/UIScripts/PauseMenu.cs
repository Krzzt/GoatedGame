using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public void ExitToMenu()
	{
		SceneManager.LoadScene("TitleScreen");
	}
	public void ExitGame()
	{
		Application.Quit();
	}
	public void ReturnToGame()
	{
		UIManager.Instance.TogglePause();
	}
}
