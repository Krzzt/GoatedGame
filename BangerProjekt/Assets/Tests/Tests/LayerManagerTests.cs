using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class LayerManagerTests : MonoBehaviour
{

	GameObject dummyObject;
	LayerManager layerManager;
	Layer testLayer;
	Layer dummyLayer;

	GameManager gameManager;
	[SetUp]
	public void Setup()
	{
		dummyObject = new GameObject();
		dummyObject.name = "manager";
		gameManager = dummyObject.AddComponent<GameManager>();
		GameManager.Instance = gameManager;
		layerManager = dummyObject.AddComponent<LayerManager>();
		layerManager.AllLayerScript = dummyObject.AddComponent<AllLayers>();
		dummyLayer = Layer.CreateInstance<Layer>();
		testLayer = Layer.CreateInstance<Layer>();
		testLayer.PossibleLayers = new List<int>
		{
			0,1
		};
		string  dummyName = dummyObject.name;
		layerManager.PermanentObjects = new List<string> { dummyName };
		dummyLayer.PossibleLayers = new List<int>
		{
			0,2
		};
		layerManager.AllLayerScript.Layers = new List<Layer>()
		{
			testLayer,
			dummyLayer
		};
	}

	[TearDown]
	public void Teardown()
	{
		LayerManager.CurrentLayerNumber = 0;
	}


	/*
	[Test]
	public void NextLayerTest()
	{
		layerManager.NextLayer();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 1);
		Assert.AreEqual(LayerManager.CurrentLayer, testLayer);
	}

	[Test]
	public void NextLayerTwiceTest()
	{
		layerManager.NextLayer();
		layerManager.NextLayer();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 2);
		Assert.AreEqual(LayerManager.CurrentLayer, dummyLayer);
	}

	[Test]
	public void NoLayerAvailableTest()
	{
		layerManager.NextLayer();
		layerManager.NextLayer();
		layerManager.NextLayer();
		layerManager.NextLayer();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 4);
		Assert.AreEqual(LayerManager.CurrentLayer, testLayer);
	}
	*/
	//play mode tests
}
