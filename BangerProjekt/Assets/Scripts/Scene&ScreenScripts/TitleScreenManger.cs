using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiteleScreenManger : MonoBehaviour 
{
    //Panel that contains the options menu
    [SerializeField] private GameObject optionspanel;

    // Show the options panel
    public void ChangeOptionsPanelState()
    {
        if (optionspanel != null)
        {
            optionspanel.SetActive(!optionspanel.activeSelf);
        }   
        else
        {
            Debug.LogError("TiteleScreenManger: Options panel is not assigned!");
        }   
}
}
