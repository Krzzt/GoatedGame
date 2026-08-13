using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	 public static UIManager Instance;
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
	private GameObject shop;
	private GameObject shopButton;
	private MainCanvasReferenceSheet referenceSheet;

    //Credits Text
    private TMP_Text creditsText;
	//End of Credits Text


	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
		referenceSheet = GameObject.FindWithTag("MainCanvas").GetComponent<MainCanvasReferenceSheet>();
		skipButton = referenceSheet.SkipWaveButton;
		waveText = referenceSheet.WaveText.GetComponent<TMP_Text>();
		enemiesAliveText = referenceSheet.EnemiesAliveText.GetComponent<TMP_Text>();

		abilityFill = referenceSheet.CooldownIndicator.GetComponent<Image>();
		SetAbilityFill(0);
		abilityImage = referenceSheet.AbilityImage.GetComponent<Image>();

		creditsText = referenceSheet.CreditsText.GetComponent<TMP_Text>();
		inventory = referenceSheet.InventoryRelated;
		shop = referenceSheet.ShopRelated;
		shopButton = referenceSheet.ShopButtonOutOfShop;
		shopButton.GetComponent<Button>().onClick.AddListener(() => this.ToggleShop());
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
		Player.ToggleShop += ToggleShop;
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

        if (inventory.activeSelf)
        {
            Player.playerInput.actions.FindAction("Fire").Disable();
            Player.playerInput.actions.FindAction("Interact").Disable();
			Player.playerInput.actions.FindAction("Toggle Shop").Disable();
			shopButton.SetActive(false);
        }
        else
        {
            Player.playerInput.actions.FindAction("Fire").Enable();
            Player.playerInput.actions.FindAction("Interact").Enable();
			Player.playerInput.actions.FindAction("Toggle Shop").Enable();
			shopButton.SetActive(true);
        }
    }

	public void ToggleShop()
	{
		shop.SetActive(!shop.activeSelf);

        if (shop.activeSelf)
        {
            Player.playerInput.actions.FindAction("Fire").Disable();
            Player.playerInput.actions.FindAction("Interact").Disable();
			Player.playerInput.actions.FindAction("Open Inventory").Disable();
        }
        else
        {
            Player.playerInput.actions.FindAction("Fire").Enable();
            Player.playerInput.actions.FindAction("Interact").Enable();
			Player.playerInput.actions.FindAction("Open Inventory").Enable();
        }
	}

}
