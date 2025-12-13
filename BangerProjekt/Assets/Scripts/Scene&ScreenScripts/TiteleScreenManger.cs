using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiteleScreenManger : MonoBehaviour
{
    //Panel die angezeigt werden sollen er zukunftig (ein verstekte UI panel)
    [SerializeField] private GameObject optionspanel;

    // Show the options panel
    public void ShowOptionsPanel()
    {
        if (optionspanel != null)
        {
            optionspanel.SetActive(true);
        }
        else
        {
            Debug.LogError("TiteleScreenManger: Options panel is not assigned!");
        }
    }

    // Hide the options panel
    public void HideOptionsPanel()
    {
        if (optionspanel != null)
        {
            optionspanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TiteleScreenManger: Options panel is not assigned!");
        }
    }
}
