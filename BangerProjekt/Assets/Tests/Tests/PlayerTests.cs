using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.InputSystem;
using TMPro;

[TestFixture]
public class PlayerTests : MonoBehaviour
{

	GameObject dummyObject;
	GameObject dummyWeaponObject;
	Player player;
	RangedWeapon dummyWeapon;
	UseAbilities dummyAbilities;
	PlayerInput dummyInput;

	GameObject dummyPopUp;

	[SetUp]
	public void Setup()
	{
		dummyObject = new GameObject();
		dummyWeaponObject = new GameObject();
		dummyWeaponObject.transform.parent = dummyObject.transform;
		dummyWeaponObject.tag = "Weapon";
		player = dummyObject.AddComponent<Player>();
		dummyWeapon = dummyWeaponObject.AddComponent<RangedWeapon>();
		dummyAbilities = dummyObject.AddComponent<UseAbilities>();
		dummyInput = dummyObject.AddComponent<PlayerInput>();

		dummyPopUp = new GameObject("PopUpPrefab");
		dummyPopUp.AddComponent<TextMeshPro>();
		dummyPopUp.AddComponent<PopUp>();

		player.AddMaxHealth(100); //100 max health yippie
	}


	[Test]
	public void DamageTest()
	{
		GameObject gameOverObject = new GameObject();
		player.GameOverScreen = gameOverObject;
		gameOverObject.SetActive(false);

		player.DamageUnit(60,1);

		Assert.IsFalse(gameOverObject.activeSelf);
		Assert.AreEqual(player.CurrentHealth, 40);
	}

	[Test]
	public void DeathTest()
	{
		GameObject gameOverObject = new GameObject();
		player.GameOverScreen = gameOverObject;
		gameOverObject.SetActive(false);

		player.DamageUnit(102, 1);

		Assert.IsTrue(gameOverObject.activeSelf);
		Assert.AreEqual(player.CurrentHealth, -2);
	}

	[Test]
	public void ImmunityFrameTest()
	{
		player.ImmuFramesOnHit = 5;
		player.DamageUnit(10,1);

		player.DamageUnit(102020202, 1);

		Assert.AreEqual(player.CurrImmunityFrames, 5);
		Assert.AreEqual(player.CurrentHealth, 90);

	}


	[Test]
	public void HealTest()
	{
		player.DamageUnit(20,1);

		player.HealUnit(10); //needs to be changed aaaaahhhh

		Assert.AreEqual(player.CurrentHealth, 90);
	}

	[Test]
	public void HealToMuchTest()
	{
		player.DamageUnit(20, 1);

		player.HealUnit(player.MaxHealth);

		Assert.AreEqual(player.CurrentHealth, player.MaxHealth);
	}

	[Test]
	public void LifeStealTest()
	{
		player.DamageUnit(10, 1);

		player.ApplyLifesteal();

		Assert.AreEqual(player.CurrentHealth, 91);
	}

	[Test]
	public void ExpAddTest()
	{
		player.AddExp(20);

		Assert.AreEqual(player.CurrentExp, 20);
		Assert.AreEqual(player.Level, 1);
	}

	[Test]
	public void LevelUpTest()
	{
		player.AddExp(50);

		Assert.AreEqual(player.CurrentExp, 0);
		Assert.AreEqual(player.Level, 2);
	}


}
