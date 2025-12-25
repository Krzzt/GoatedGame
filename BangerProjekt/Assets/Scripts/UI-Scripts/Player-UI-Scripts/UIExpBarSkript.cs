using System.Collections;
using System.Collections.Generic;
//Unity specific libraries
using UnityEngine;
//UI library is needed for Image elements
using UnityEngine.UI;
//TextMeshPro is needed for text elements
using TMPro;
using TMPro.EditorUtilities;

public class UIExpBarSkript : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] public Image barFillImage;
    [SerializeField] public TextMeshProUGUI expText;
    [SerializeField] public TextMeshProUGUI levelText;

    //Becouse the UI can be enabled and disabled durig gameplay we need to subscribe and unsubscribe to the event here
    void OnEnable()
    {
        if (player != null)
        {
            //update the exp bar when the event is triggered
            player.OnExpChanged += UpdateExpBar;
            player.OnLevelChanged += UpdateLevelBar;

        }
    }
    void OnDisable()
    {
        if (player != null)
        {
            //unsubscribe from the event when disabled to prevent memory leaks
            player.OnExpChanged -= UpdateExpBar;
            player.OnLevelChanged -= UpdateLevelBar;
        }
    }
    private void Start()
    {
        // Initialize state (if Event comes befoere UI is ready)
        UpdateExpBar(player.CurrentExp, player.RequiredExp);
        UpdateLevelBar(player.Level);
    }
    void UpdateExpBar(int currentExp, int requiredExp)
    {
        if (requiredExp <= 0)
        {
            //prevent division by zero
            barFillImage.fillAmount = 0f;
            //update text
            expText.text = "EXP 0 / 0";
            return;
        }
        //calculate fill amount
        float fillAmount = (float)currentExp / requiredExp;
        barFillImage.fillAmount = fillAmount;

        //update text
        expText.text = currentExp.ToString() + " / " + requiredExp.ToString() + " EXP";
    }

    void UpdateLevelBar(int level)
    {
        //update level text
        levelText.text = "Level " + level.ToString();
    }

}
