using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DeckLogic : MonoBehaviour
{
    private List<Card> entireDeck = new List<Card>();
    private List<Card> drawPile = new List<Card>();
    private List<Card> cardsInHand = new List<Card>();
    private List<Card> discardPile = new List<Card>();

    [SerializeField] private const int MAX_CARDS = 13; //13 is hard cap
    [SerializeField] private int drawAmount;
    private AllCards allCardList;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardScreen;

    [SerializeField] private int roundCurrency;
    private int currencyAmount;

    private TMP_Text currencyText;
    private void Awake()
    {
        allCardList = gameObject.GetComponent<AllCards>(); // gameObject with small g since this Object holds both
        drawPile.AddRange(entireDeck);
        ShuffleDrawPile();
        //DrawCards(drawAmount); //we fill the hand with cards
    }

    private void OnEnable()
    {
        SaveManager.SavingGame += SaveCards;
        SaveManager.LoadingGame += LoadCards;
        ShopHover.purchaseCard += AddCard;
        LayerManager.newLayer += StartTurn;
        CardInHand.CardPlayed += PlayCard;
    }

    private void OnDisable()
    {
        SaveManager.SavingGame -= SaveCards;
        SaveManager.LoadingGame -= LoadCards;
        ShopHover.purchaseCard -= AddCard;
        LayerManager.newLayer -= StartTurn;
        CardInHand.CardPlayed -= PlayCard;

    }

    public void AddCard(Card newCard)
    {
        entireDeck.Add(newCard);
        drawPile.Add(newCard);
        ShuffleDrawPile(); //because we added a new card to the drawpile, we shuffle it again so the new card can appear at any point
        //but the discard pile stays where it is
    }
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount && cardsInHand.Count < MAX_CARDS && (drawPile.Count > 0 || discardPile.Count > 0); i++) //for every amount, we draw 1 Card. Alternatively, stop if the hand is "full"
        {
            Debug.Log("Sent" + i);
            if (drawPile.Count <= 0) //if the drawPile is empty
            {
                RecycleDiscardPile(); //Recycle the Discard Pile
            }
            cardsInHand.Add(drawPile[0]);  //we draw a card by adding it to our HandList and removing index 0 from the draw List
            drawPile.RemoveAt(0);
        }
        DebugHand();
    }

    public void RecycleDiscardPile() //to shuffle the discard pile back into the drawPile
    {
        drawPile.AddRange(discardPile); //this adds the entire discardPile List to the drawPile List (List.AddRange)
        discardPile.Clear(); //this clears the discardPile List
        ShuffleDrawPile();
    }

    public void ShuffleDrawPile()
    {
        for (int i = 0; i < drawPile.Count - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, drawPile.Count - i);
            drawPile.Add(drawPile[randomIndex]);
            drawPile.RemoveAt(randomIndex);
            //Okay lets explain this process
            //We want to shuffle our drawPile. To do this, we search 1 random index between 0 and the last one (-i)
            //The card at this index gets moved to last position by adding it (so its last in the list) and removing its instance at said index
            //This effectively moves the card to the last position in the list
            //after this, we only want to regard the cards that dont have been shuffled yet, hence the -i to remove those from the potential random indexes
            //and in our for we can save one operation by using drawpile.Count -1 since every card has been "shuffled" except for the last one, which is randomly decided
            //hence making it random
        }
    }

    public void PlayCard(int cardIDinHand)
    {
        //do a cool effect based in the ID of the card
        //would look probably like switch(cardsInHand[indexInHand].ID) {
        //case 0: .... break;
        //case 1: .... break;
        //....
        //}

        //also check for currency and stuff
        Card card = cardsInHand[cardIDinHand];
        if (currencyAmount < card.CurrencyCost) return;
        currencyAmount -= card.CurrencyCost;
        switch(card.ID)
        {
            case 0:
                //cool stuff based on ID
            break;
        }
        DiscardCard(cardIDinHand);
        if (currencyText) currencyText.SetText("Currency: " + currencyAmount + "/" + roundCurrency);


    }

    public void DiscardCard(int IDtoDiscard)
    {
        Card cardToDiscard = cardsInHand[IDtoDiscard];
        discardPile.Add(cardToDiscard);
        cardsInHand.RemoveAt(IDtoDiscard); //to prevent deleting a copy instead of the right one if 2 of the same kind are in 1 hand
        SetCardUI();
    }

    public void DiscardHand()
    {
        while(cardsInHand.Count > 0)    DiscardCard(0); //always delete 0 should work
    }
    public void DebugHand() //this is a Debug function to just show every card in hand by name
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Debug.Log("Card " + i + ": " + cardsInHand[i].Name);
        }
    }

    private void SaveCards()
    {
        SaveManager.currentSave.CardsInHand = cardsInHand;
        SaveManager.currentSave.EntireDeck = entireDeck;
        SaveManager.currentSave.DrawPile = drawPile;
        SaveManager.currentSave.DiscardPile = discardPile;
    }

    private void LoadCards()
    {
        cardsInHand = SaveManager.currentSave.CardsInHand;
        entireDeck = SaveManager.currentSave.EntireDeck;
        drawPile = SaveManager.currentSave.DrawPile;
        foreach(Card card in drawPile)
        {
            Debug.Log("Card!");
        }
        discardPile = SaveManager.currentSave.DiscardPile;
        if (discardPile.Count <= 0) ShuffleDrawPile(); //if nothing is discarded, its a new game so shuffle (and if its not it doesnt matter anyways)
    }


    public void StartTurn()
    {
        currencyAmount = roundCurrency;
        DrawCards(drawAmount); //draw as many cards as
        Time.timeScale = 0; //Scary oooooo
        GameObject newScreen = Instantiate(cardScreen, GameObject.FindWithTag("MainCanvas").transform);
        newScreen.GetComponentInChildren<Button>().onClick.AddListener(() => EndTurn());
        //Instantiate the Shit
        //the script inside the screen handles the rendering? maybe event, maybe do everything here?
        SetCardUI();
        currencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        currencyText.SetText("Currency: " + currencyAmount + "/" + roundCurrency);
    }

    public void SetCardUI()
    {
        List<GameObject> cardObjects = GameObject.FindGameObjectsWithTag("Card").ToList();
        foreach(GameObject card in cardObjects)
        {
            Destroy(card);
        }
        int counter = 0;
        foreach(Card card in cardsInHand)
        {
            GameObject cardObject = Instantiate(cardPrefab, GameObject.FindWithTag("CardSelect").GetComponentInChildren<LayoutGroup>().transform);
            cardObject.name = "Card_" + counter;
            cardObject.transform.Find("CardEffectImage").GetComponent<Image>().sprite = card.CardImage; //child 0 = image
            cardObject.transform.Find("CardBackgroundImage").GetComponent<Image>().sprite = card.LayerOfCard.CardBackground[(int)card.CardRarity + 1];
            cardObject.transform.Find("CardName").GetComponent<TMP_Text>().SetText(card.Name);
            cardObject.transform.Find("CardDescription").GetComponent<TMP_Text>().SetText(card.Description); 
            cardObject.transform.Find("CurrencyCostImage").GetChild(0).GetComponentInChildren<TMP_Text>().SetText(card.CurrencyCost.ToString()); 
            CardInHand cardScript = cardObject.AddComponent<CardInHand>();
            cardScript.CardSO = card;
            cardScript.cardInHandID = counter;
            counter++;
        }
    }
    public void EndTurn()
    {
        DiscardHand();
        Time.timeScale = 1; //maybe need a check bcs of pausing and shit
        Destroy(GameObject.FindWithTag("CardSelect"));
    }
}
