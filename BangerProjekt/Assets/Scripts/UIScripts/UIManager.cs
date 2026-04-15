using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //WAVE RELATED
    private TMP_Text waveText;
    private TMP_Text enemiesAliveText;
    private GameObject skipButton;
    //END OF WAVE RELATED

    //ABILITY RELATED
    private Image abilityFill;
    private Image abilityImage;
    //END OF ABILITY RELATED

    //INVENTORY RELATED
    private GameObject inventory;
    //END OF INVENTORY RELATED

    //Credits Text
    private TMP_Text creditsText;
    //End of Credits Text


    private void Awake()
    {
        skipButton = GameObject.FindWithTag("SkipWaveButton");
        //skipButton.SetActive(false);
        waveText = GameObject.FindWithTag("WaveText").GetComponent<TMP_Text>(); //i prefer tags since they dont change as often as names
        //waveText.gameObject.SetActive(false);
        enemiesAliveText = GameObject.FindWithTag("EnemiesAliveText").GetComponent<TMP_Text>();
        //enemiesAliveText.gameObject.SetActive(false);
        abilityFill = GameObject.FindWithTag("CooldownIndicator").GetComponent<Image>();
        SetAbilityFill(0);
        abilityImage = GameObject.FindWithTag("AbilityImage").GetComponent<Image>();

        creditsText = GameObject.FindWithTag("CreditsText").GetComponent<TMP_Text>();
        inventory = GameObject.FindWithTag("Inventory");

    }
    private void OnEnable()
    {
        RoomScript.StartWaves += SetWaveVisible;
        RoomScript.RoomCleared += SetWaveInvisible;
        EnemySpawner.NewWaveText += SetWaveText;
        EnemySpawner.NewEnemiesRemaining += SetEnemiesAliveText;
        EnemySpawner.LastWave += DisableButton;
        UseAbilities.SetAbilityUI += SetAbilityFill;
        Player.NewAbility += SetAbilityImage;
        GameManager.CreditsChanged += SetCreditText;
        Player.ToggleInventory += ToggleInventory;
    }
    public void SetWaveText(int currWave, int maxWave)
    {
        waveText.SetText("Wave " + currWave + "/" + maxWave);
    }

    public void SetWaveVisible(int dontCare)
    {
        if (waveText && enemiesAliveText && skipButton)
        {
            waveText.gameObject.SetActive(true);
            enemiesAliveText.gameObject.SetActive(true);
            SetEnemiesAliveText(0); //reset it yes
            skipButton.SetActive(true);
        }
    }

    public void DisableButton()
    {
        if (skipButton) skipButton.SetActive(false);

    }

    public void SetWaveInvisible()
    {
        if (waveText && enemiesAliveText && skipButton)
        {
            waveText.gameObject.SetActive(false);
            enemiesAliveText.gameObject.SetActive(false);
            skipButton.SetActive(false);
        }

    }

    public void SetEnemiesAliveText(int amount)
    {
        enemiesAliveText.SetText("Enemies Remaining: " + amount);
    }

    public void SetAbilityFill(float fillAmount)
    {
        if (abilityFill) abilityFill.fillAmount = fillAmount;

    }

    public void SetAbilityImage(AbilityItem item)
    {
        abilityImage.sprite = item.Icon;
    }

    public void SetCreditText()
    {
        creditsText.SetText("Credits: " + GameManager.credits);
    }

    public void ChangeInput(InputAction inputToChange)
    {
        var rebindOperation = inputToChange.PerformInteractiveRebinding().WithCancelingThrough("<Keyboard>/escape"); //cancelled with escape
        rebindOperation.OnMatchWaitForAnother(0.1f); //to wait shortly after the input is pressed
        rebindOperation.Start();
    }

    public void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }


}
