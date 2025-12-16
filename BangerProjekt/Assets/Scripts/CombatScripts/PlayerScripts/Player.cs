using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Start of Health variables --------------------------------
    public UnitHealth PlayerHealth = new UnitHealth(100, 100); //Player starts with 100HP (just example, can be changed anytime)
    //End of Health variables ----------------------------------

    //Start of Card variables --------------------------------
    public List<Card> entireDeck = new List<Card>();
    public List <Card> drawPile = new List<Card>();
    public List <Card> cardsInHand = new List<Card>();
    public List <Card> discardPile = new List<Card>();

    public int handSize;
    //End of Card variables ---------------------------------

    //Start of level variables ------------------------------
    private int level;
    private int currentExp = 0;
    public int CurrentExp
    {
        get
        {
            return currentExp;
        }
        set
        {
            currentExp = value;
        }
    }
    private int requiredExp = 10;
    public int RequiredExp
    {
        get
        {
            return requiredExp;
        }
        set
        {
            requiredExp = value;
        }
    }
    //End of level variables -------------------------------




    //_______________________________________________________________________________________________________________
    //START OF FUNCTIONS

    //Start of Unity specific functions ----------------------------
    void Awake()
    {
        for (int i = 0; i < 10; i++) // for testing, we initialize the entire deck with 5 cards of 2 different types
        {
            if (i % 2 == 0)
            {
                entireDeck.Add(CardList.allCards[0]);
            }
            else
            {
                entireDeck.Add(CardList.allCards[1]);
            }
        }
        drawPile = entireDeck; //all cards go into the drawPile
        ShuffleDeck();
        DrawCards(handSize); //we fill the hand with cards
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(3);
        }
    }
    //End of Unity specific functions ----------------------------


    //Start of HP related functions -----------------------------
    public void TakeDamage(int amount)
    {
        PlayerHealth.DamageUnit(amount);
        if (PlayerHealth.MaxHealth <= 0)
        {
            //Die
        }
    }

    public void Heal(int amount)
    {
        PlayerHealth.HealUnit(amount);
    }

    //End of HP related functions --------------------------------

    //Start of Card functions -------------------------------------
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount && cardsInHand.Count < handSize; i++) //for every amount, we draw 1 Card. Alternatively, stop if the hand is "full"
        {
            if (drawPile.Count > 0) //if the drawPile is not empty
            {
                cardsInHand.Add(drawPile[0]); 
                drawPile.RemoveAt(0);
                //we draw a card by adding it to our HandList and removing index 0 from the draw List
            }
            else //if the drawPile is empty
            {
                RecycleDeck();
                cardsInHand.Add(drawPile[0]); 
                drawPile.RemoveAt(0);

            }

        }
        //DebugHand();
    }

    public void RecycleDeck() //to shuffle the discard pile back into the drawPile
    {
        drawPile.AddRange(discardPile); //this adds the entire discardPile List to the drawPile List (List.AddRange)
        discardPile.Clear(); //this clears the discardPile List
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < drawPile.Count-1 ; i++) 
        {
            int randomIndex = UnityEngine.Random.Range(0,drawPile.Count - i);
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

    public void PlayCard(int indexInHand)
    {
        //do a cool effect based in the ID of the card
        //would look probably like switch(cardsInHand[indexInHand].ID) {
        //case 0: .... break;
        //case 1: .... break;
        //....
        //}

        discardPile.Add(cardsInHand[indexInHand]);
        cardsInHand.RemoveAt(indexInHand);
    }

    public void DebugHand() //this is a Debug function to just show every card in hand by name
    {
        for (int i = 0; i < cardsInHand.Count ; i++)
        {
            Debug.Log("Card " + i + ": " + cardsInHand[i].name);
        }
    }

    //End of Card functions -------------------------------------------------

    //Start of exp related functions -----------------------------------------

    public void AddExp(int amount)
    {
        CurrentExp += amount; 
        while(CurrentExp >= RequiredExp)
        {
            CurrentExp -= RequiredExp;
            LevelUp();
            //as long as a level up should happen, we let it happen (the required exp go up in the lvlup function)
            //We also must subtract from the Current exp to prevent an infinite loop
        }
        
    }

    public void LevelUp()
    {
        level++;
        RequiredExp = (int)(RequiredExp * 1.5f); //for now, nothing happens on level up, its only a number going up
        //and the required exp goes up by 1.5 (rounded down to the nearest integer)


        //Increase stats
    }

    //End of exp related functions --------------------------------------
}
