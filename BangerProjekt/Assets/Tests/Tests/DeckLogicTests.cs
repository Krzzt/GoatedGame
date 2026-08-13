using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


[TestFixture]
public class DeckLogicTests
{
	GameObject dummyObject;
	Card dummyCard;
	DeckLogic logic;

	[SetUp]
	public void Setup()
	{
		dummyObject = new GameObject();
		dummyCard = ScriptableObject.CreateInstance<Card>();
		logic = dummyObject.AddComponent<DeckLogic>();
		while (logic.EntireDeck.Count < 5) logic.AddCard(dummyCard);
		//now we have 5 dummycards
	}

	[TearDown]
	public void Teardown()
	{
		//not needed rn
	}

	[Test]
	public void DrawAllCardsTest()
	{
		logic.DrawCards(5);

		Assert.AreEqual(logic.CardsInHand.Count, 5);
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}

	[Test]
	public void DrawMoreCardsThanAllowedTest()
	{
		logic.DrawCards(6);

		Assert.AreEqual(logic.CardsInHand.Count, 5);
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}

	[Test]
	public void MaxHandCardAmountTest()
	{
		while (logic.EntireDeck.Count < DeckLogic.MAX_CARDS + 2) logic.AddCard(dummyCard);

		logic.DrawCards(logic.EntireDeck.Count);

		Assert.AreEqual(logic.CardsInHand.Count, DeckLogic.MAX_CARDS);
		Assert.AreEqual(logic.DrawPile.Count, 2); //still 2 cards left since we cant draw

	}

	[Test]
	public void DrawWhenNothingLeftTest()
	{
		logic.DrawCards(5);

		logic.DrawCards(1);

		Assert.AreEqual(logic.CardsInHand.Count, 5);
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}

	[Test]
	public void DiscardHandTest()
	{
		logic.DrawCards(5);

		logic.DiscardHand();

		Assert.AreEqual(logic.CardsInHand.Count, 0);
		Assert.AreEqual(logic.DiscardPile.Count, 5); //gets mixed back when trying to draw more
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}

	[Test]
	public void DiscardSingleCardTest()
	{
		logic.DrawCards(5);

		logic.DiscardCard(0); //index

		Assert.AreEqual(logic.CardsInHand.Count, 4);
		Assert.AreEqual(logic.DiscardPile.Count, 1);
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}

	[Test]
	public void RecycleDiscardPileTest()
	{
		logic.DrawCards(5);
		logic.DiscardHand();

		logic.DrawCards(5);

		Assert.AreEqual(logic.CardsInHand.Count, 5);
		Assert.AreEqual(logic.DiscardPile.Count, 0);
		Assert.AreEqual(logic.DrawPile.Count, 0);
	}
}
