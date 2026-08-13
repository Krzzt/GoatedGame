using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using TMPro;
using UnityEngine.InputSystem;

[TestFixture]
public class EnemyTests 
{
	GameObject dummyEnemyObject;
	Enemy enemy;
	Rigidbody2D dummyrb;

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
		dummyEnemyObject = new GameObject();
		enemy = dummyEnemyObject.AddComponent<Enemy>();
		dummyrb = dummyEnemyObject.AddComponent<Rigidbody2D>();

		enemy.AddMaxHealth(20);

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


		enemy.playerObject = dummyObject;
	}


	[Test]
	public void DamageUnitTest()
	{
		enemy.DamageUnit(10,1);

		Assert.AreEqual(enemy.CurrentHealth, 10);
	}

	/*
	[Test]
	public void DeathTest()
	{
		enemy.DamageUnit(20, 1);

		Assert.AreEqual(enemy.CurrentHealth, 0);
		Assert.AreEqual(player.KillCount, 1);
	}
	*/
	//play mode tests!
}
