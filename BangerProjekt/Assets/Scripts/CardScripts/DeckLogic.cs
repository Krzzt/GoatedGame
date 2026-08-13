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
	public static DeckLogic Instance;

	public List<Card> EntireDeck { get; private set; } = new List<Card>();
	public List<Card> DrawPile { get; private set; } = new List<Card>();
	public List<Card> CardsInHand { get; private set; } = new List<Card>();
	public List<Card> DiscardPile { get; private set; } = new List<Card>();
	private List<Card> activeCards = new List<Card>();

	public const int MAX_CARDS = 13; //13 is hard cap, also public because setting it isnt possible
	[SerializeField] private int drawAmount;
	private AllCards allCardList;

	[SerializeField] private GameObject cardPrefab;
	[SerializeField] private GameObject cardScreen;

	[SerializeField] private int roundCurrency;
	public int CurrencyAmount { get; set; }

	private TMP_Text currencyText;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);


		allCardList = gameObject.GetComponent<AllCards>(); // gameObject with small g since this Object holds both
		DrawPile.AddRange(EntireDeck);
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
		RoomScript.RoomCleared += OnRoomClearCardEffects;
	}

	private void OnDisable()
	{
		SaveManager.SavingGame -= SaveCards;
		SaveManager.LoadingGame -= LoadCards;
		ShopHover.purchaseCard -= AddCard;
		LayerManager.newLayer -= StartTurn;
		CardInHand.CardPlayed -= PlayCard;
		RoomScript.RoomCleared -= OnRoomClearCardEffects;
	}

	public void OnDestroy()
	{
		Instance = null;
	}

	public void AddCard(Card newCard)
	{
		EntireDeck.Add(newCard);
		DrawPile.Add(newCard);
		ShuffleDrawPile(); //because we added a new card to the drawpile, we shuffle it again so the new card can appear at any point
						   //but the discard pile stays where it is
	}
	public void DrawCards(int amount)
	{
		for (int i = 0; i < amount && CardsInHand.Count < MAX_CARDS && (DrawPile.Count > 0 || DiscardPile.Count > 0); i++) //for every amount, we draw 1 Card. Alternatively, stop if the hand is "full"
		{
			if (DrawPile.Count <= 0) //if the drawPile is empty
			{
				RecycleDiscardPile(); //Recycle the Discard Pile
			}
			CardsInHand.Add(DrawPile[0]);  //we draw a card by adding it to our HandList and removing index 0 from the draw List
			DrawPile.RemoveAt(0);
		}
		//DebugHand();
	}

	public void RecycleDiscardPile() //to shuffle the discard pile back into the drawPile
	{
		DrawPile.AddRange(DiscardPile); //this adds the entire discardPile List to the drawPile List (List.AddRange)
		DiscardPile.Clear(); //this clears the discardPile List
		ShuffleDrawPile();
	}

	public void ShuffleDrawPile()
	{
		for (int i = 0; i < DrawPile.Count - 1; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, DrawPile.Count - i);
			DrawPile.Add(DrawPile[randomIndex]);
			DrawPile.RemoveAt(randomIndex);
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
		Card card = CardsInHand[cardIDinHand];
		if (CurrencyAmount < card.CostToPlay) return;
		CurrencyAmount -= card.CostToPlay;
		foreach (Pair<CardEffect, string> pair in card.CardEffects)
		{
			pair.First.ExecuteEffect(pair.Second);
		}
		activeCards.Add(card);
		DiscardCard(cardIDinHand);
		if (currencyText) currencyText.SetText("Currency: " + CurrencyAmount + "/" + roundCurrency);
		SetCardUI();

	}

	public void OnRoomClearCardEffects()
	{
		foreach (Card card in activeCards)
		{
			foreach (Pair<CardEffect, string> pair in card.CardEffects)
			{
				pair.First.OnRoomClear();
			}
		}
	}

	public void ResetCardEffects()
	{
		//reset effects foreach card in the list
		foreach (Card card in activeCards)
		{
			foreach (Pair<CardEffect, string> pair in card.CardEffects)
			{
				pair.First.RevertEffect(pair.Second);
			}
		}
		activeCards.Clear();
	}

	public void DiscardCard(int IDtoDiscard)
	{
		Card cardToDiscard = CardsInHand[IDtoDiscard];
		DiscardPile.Add(cardToDiscard);
		CardsInHand.RemoveAt(IDtoDiscard); //to prevent deleting a copy instead of the right one if 2 of the same kind are in 1 hand
	}

	public void DiscardHand()
	{
		while (CardsInHand.Count > 0) DiscardCard(0); //always delete 0 should work
	}
	public void DebugHand() //this is a Debug function to just show every card in hand by name
	{
		for (int i = 0; i < CardsInHand.Count; i++)
		{
			Debug.Log("Card " + i + ": " + CardsInHand[i].Name);
		}
	}

	private void SaveCards()
	{
		SaveManager.currentSave.CardsInHand = CardsInHand;
		SaveManager.currentSave.EntireDeck = EntireDeck;
		SaveManager.currentSave.DrawPile = DrawPile;
		SaveManager.currentSave.DiscardPile = DiscardPile;
	}

	private void LoadCards()
	{
		CardsInHand = SaveManager.currentSave.CardsInHand;
		EntireDeck = SaveManager.currentSave.EntireDeck;
		DrawPile = SaveManager.currentSave.DrawPile;
		DiscardPile = SaveManager.currentSave.DiscardPile;
		if (DiscardPile.Count <= 0) ShuffleDrawPile(); //if nothing is discarded, its a new game so shuffle (and if its not it doesnt matter anyways)
	}


	public void StartTurn()
	{

		CurrencyAmount = roundCurrency;
		ResetCardEffects();
		DrawCards(drawAmount); //draw as many cards as
		Time.timeScale = 0; //Scary oooooo
		GameObject newScreen = Instantiate(cardScreen, GameObject.FindWithTag("MainCanvas").transform);
		newScreen.GetComponentInChildren<Button>().onClick.AddListener(() => EndTurn());
		//Instantiate the Shit
		//the script inside the screen handles the rendering? maybe event, maybe do everything here?
		SetCardUI();
		currencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
		currencyText.SetText("Currency: " + CurrencyAmount + "/" + roundCurrency);
	}

	public void SetCardUI()
	{
		List<GameObject> cardObjects = GameObject.FindGameObjectsWithTag("Card").ToList();
		foreach (GameObject card in cardObjects)
		{
			Destroy(card);
		}
		int counter = 0;
		foreach (Card card in CardsInHand)
		{
			GameObject cardObject = Instantiate(cardPrefab, GameObject.FindWithTag("CardSelect").GetComponentInChildren<LayoutGroup>().transform);
			cardObject.name = "Card_" + counter;
			cardObject.transform.Find("CardEffectImage").GetComponent<Image>().sprite = card.CardImage; //child 0 = image
			cardObject.transform.Find("CardBackgroundImage").GetComponent<Image>().sprite = card.LayerOfCard.CardBackground[(int)card.CardRarity + 1];
			cardObject.transform.Find("CardName").GetComponent<TMP_Text>().SetText(card.Name);
			cardObject.transform.Find("CardDescription").GetComponent<TMP_Text>().SetText(card.Description);
			cardObject.transform.Find("CurrencyCostImage").GetChild(0).GetComponentInChildren<TMP_Text>().SetText(card.CostToPlay.ToString());
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
