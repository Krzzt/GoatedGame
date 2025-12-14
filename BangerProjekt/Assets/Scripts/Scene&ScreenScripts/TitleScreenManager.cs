using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour 
{
    //Panel that contains the options menu
    [SerializeField] private GameObject optionsPanel;

    // Show the options panel
    public void ChangeOptionsPanelState()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }   
        else
        {
            Debug.LogError("TitleScreenManager: Option panel is not assigned!");
        }   
    }
}
