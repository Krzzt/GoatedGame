using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class TableScript : MonoBehaviour
{
    [field:SerializeField] private GameObject cardPrefab;
    [field:SerializeField] private Transform table;

    void OnEnable()
    {
        ShopManager.sendCardsToTable += PlaceCardsOnTable;
    }
    void OnDisable()
    {
        ShopManager.sendCardsToTable -= PlaceCardsOnTable;
    }

    void PlaceCardsOnTable(List<Card> cards)
    {
        Debug.Log("PlacingCards");
        int cardNumber = 0;
        foreach(Card card in cards)
        {
            GameObject GUIcard = Instantiate(cardPrefab, table, false);
            GUIcard.name = "Card_" + ++cardNumber;
            GUIcard.transform.Find("CardBackgroundImage").GetComponent<Image>().sprite = LayerManager.CurrentLayer.CardBackround[(int)card.CardRarity + 1]; //+1 because 0 is the backside
            GUIcard.transform.Find("CardEffectImage").GetComponent<Image>().sprite = card.CardImage;
            GUIcard.transform.Find("CardName").GetComponent<TMP_Text>().SetText(card.Name);
            GUIcard.transform.Find("CardDescription").GetComponent<TMP_Text>().SetText(card.Description);
            GUIcard.transform.localScale = new Vector3(1, 1, 1);
            //GUIcard.transform.SetParent(table);
        }
    }
}
