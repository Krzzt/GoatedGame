using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStateManager : MonoBehaviour 
{
    //Panel that contains something like options
    [SerializeField] private GameObject PanelstateManager;

    // Show the panel
    public void ChangePanelState()
    {
        if (PanelstateManager != null)
        {
            PanelstateManager.SetActive(!PanelstateManager.activeSelf);
        }   
        else
        {
            Debug.LogError("PanelStateManager: Option panel is not assigned!");
        }   
}
}
