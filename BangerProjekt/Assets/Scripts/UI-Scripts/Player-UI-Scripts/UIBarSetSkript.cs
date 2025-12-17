using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarSetSkript : MonoBehaviour
{
    [SerializeField] public Unit targetScript;
    [SerializeField] public Image BarFillImage;
    // Update is called once per frame
    void Update()
    {
        if (targetScript == null || BarFillImage == null)
        {
         Debug.LogWarning("UIBarSetSkript: TargetScript or BarFillImage is not assigned.");
        }

        float currentValue = 0f;
        float maxValue = 1f;

        if (targetScript is Unit health)
        {
            
            currentValue = health.CurrentHealth;
            maxValue = health.MaxHealth;
        }
        
        else
        {
            Debug.LogWarning("UIBarSetSkript: TargetScript is not of type UnitHealth.");
        }   

    }
}
