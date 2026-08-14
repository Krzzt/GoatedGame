using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestRunner;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using TMPro;
using NavMeshPlus.Components;
using UnityEngine.SceneManagement;
using NavMeshPlus.Extensions;
using System;

[TestFixture]
public class EnemyPlayModeTests
{

	GameObject dummyEnemy;
	BasicEnemy dummyEnemyScript;

	GameObject dummySurfaceManager;
	NavMeshSurface dummySurface;

	[SetUp]
	public void Setup()
	{
		SceneManager.LoadScene("PlayTestScene");
		dummySurfaceManager = new GameObject();
		dummySurface = dummySurfaceManager.AddComponent<NavMeshSurface>();
		dummySurfaceManager.AddComponent<CollectSources2d>();
		dummySurface.BuildNavMesh();


	}


	[UnityTest]
	public IEnumerator EnemyDamageTest()
	{

		dummyEnemy = new GameObject();
		dummyEnemyScript = dummyEnemy.AddComponent<BasicEnemy>();
		dummyEnemy.transform.position = new Vector3(2, 2, 0);
		dummyEnemyScript.AddMaxHealth(20);

		dummyEnemyScript.DamageUnit(10, 1);

		yield return new WaitForSeconds(0);
		Assert.AreEqual(dummyEnemyScript.CurrentHealth, 10);
	}


	[UnityTest]
	public IEnumerator EnemyDeathTest()
	{

		dummyEnemy = new GameObject();
		dummyEnemyScript = dummyEnemy.AddComponent<BasicEnemy>();
		dummyEnemy.transform.position = new Vector3(2, 2, 0);
		dummyEnemyScript.AddMaxHealth(20);

		dummyEnemyScript.DamageUnit(30, 1);

		yield return new WaitForEndOfFrame();
		Assert.IsTrue(dummyEnemy == null);
	}
}
