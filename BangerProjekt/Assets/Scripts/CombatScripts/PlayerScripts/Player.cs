using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{


    //Start of Card variables --------------------------------
    private List<Card> entireDeck = new List<Card>();
    private List <Card> drawPile = new List<Card>();
    private List <Card> cardsInHand = new List<Card>();
    private List <Card> discardPile = new List<Card>();

    [SerializeField] private int handSize;
    //End of Card variables ---------------------------------

    //Start of level variables ------------------------------
    private int level;
    private int currentExp = 0;
    private int requiredExp = 10;
    //End of level variables -------------------------------

    //Start of general Player variables ----------------------

     [NonSerialized] public int killCount; //THIS IS PUBLIC
    //like every enemy script wnats to access this it just makes sense

    //End of general Player variables -------------------------

    //_______________________________________________________________________________________________________________
    //START OF FUNCTIONS

    //Start of Unity specific functions ----------------------------
    void Awake()
    {
        MaxHealth = CurrentHealth; //set ur health
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
        ShuffleDrawPile();
        DrawCards(handSize); //we fill the hand with cards
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //if the collision is an enemy (as seen by its tag)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy>().Damage);
        }
    }
    //End of Unity specific functions ----------------------------


    //Start of HP related functions -----------------------------
    public void TakeDamage(int amount)
    {
        //This damage currently does not involve something like immunity frames or shit like that
        //also every enemy damages you on collision, if you hug them forever, you only take damage once!
        DamageUnit(amount);
        Debug.Log("took damage!");
        //Update the Healthbar if existent
        if (MaxHealth <= 0)
        {
            Debug.Log("you should be dead");
            //Die
        }
    }

    public void Heal(int amount)
    {
        HealUnit(amount);
        //Update the healthbar if existent
    }

    //End of HP related functions --------------------------------

    //Start of Card functions -------------------------------------
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount && cardsInHand.Count < handSize; i++) //for every amount, we draw 1 Card. Alternatively, stop if the hand is "full"
        {
            if (drawPile.Count <= 0) //if the drawPile is empty
            {
                RecycleDiscardPile(); //Recycle the Discard Pile

              
            }
            cardsInHand.Add(drawPile[0]);  //we draw a card by adding it to our HandList and removing index 0 from the draw List
            drawPile.RemoveAt(0);

        }
        //DebugHand();
    }

    public void RecycleDiscardPile() //to shuffle the discard pile back into the drawPile
    {
        drawPile.AddRange(discardPile); //this adds the entire discardPile List to the drawPile List (List.AddRange)
        discardPile.Clear(); //this clears the discardPile List
        ShuffleDrawPile();
    }

    public void ShuffleDrawPile()
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
            Debug.Log("Card " + i + ": " + cardsInHand[i].Name);
        }
    }

    //End of card related functions


    //Start of exp Related functions

    public void AddExp(int amount)
    {
        currentExp += amount;
        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }
        //this while loop is here to make multiple level ups possible
    }

    public void LevelUp()
    {
        level++;
        requiredExp = (int)(requiredExp * 1.5f);
        //for now, this is just a number going up and the exp also going up

        //stat increase probably
    }

    //end of exp related functions -----------------------
}
