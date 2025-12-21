using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarSetSkript : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] public Image barFillImage;
    
    //Becouse the UI can be enabled and disabled durig gameplay we need to subscribe and unsubscribe to the event here
    //
    void OnEnable()
    {
        if (player != null)
        {
            //update the health bar when the event is triggered
            player.OnHealthChanged += UpdateBar;
            
        }
    }


    void OnDisable()
    {
        if (player != null)
        {
            //unsubscribe from the event when disabled to prevent memory leaks
            player.OnHealthChanged -= UpdateBar;
        }
    }

    private void Start()
    {
        // Initialize state (if Event comes befoere UI is ready)
        UpdateBar(player.CurrentHealth, player.MaxHealth);
    }

    void UpdateBar(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
        {
            barFillImage.fillAmount = 0f;
            return;
        }
        //calculate fill amount
        float fillAmount = (float)currentHealth / maxHealth;
        barFillImage.fillAmount = fillAmount;
    }


}   
