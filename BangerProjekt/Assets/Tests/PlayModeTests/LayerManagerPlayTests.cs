using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
public class LayerManagerPlayTests
{
	GameObject layerManagerObject;
	LayerManager layerManagerScript;

	Layer dummyLayer;
	Layer testLayer;


	[UnitySetUp]
	public IEnumerator Setup()
	{
		if (File.Exists(Application.dataPath + "/saves/saveFile.json"))
		{
			File.Delete(Application.dataPath + "/saves/saveFile.json");
		}

		yield return SceneManager.LoadSceneAsync("LayerManagerTestScene", LoadSceneMode.Single);
		layerManagerObject = GameObject.Find("LayerManager");
		layerManagerScript = layerManagerObject.GetComponent<LayerManager>();
		dummyLayer = Layer.CreateInstance<Layer>();
		testLayer = Layer.CreateInstance<Layer>();
		testLayer.PossibleLayers = new List<int>
		{
			0,2
		};
		dummyLayer.PossibleLayers = new List<int>
		{
			0,3
		};
		layerManagerScript.AllLayerScript.Layers = new List<Layer>()
		{
			testLayer,
			dummyLayer
		};
		LayerManager.CurrentLayerNumber = 1;
		//TODO: DummySaveFile! / erase save file
	}




	[UnityTest]
	public IEnumerator NextLayerTest()
	{
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 2);
		Assert.AreEqual(LayerManager.CurrentLayer, testLayer);
	}

	[UnityTest]
	public IEnumerator NextLayerTwiceTest()
	{
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 3);
		Assert.AreEqual(LayerManager.CurrentLayer, dummyLayer);
	}

	[UnityTest]
	public IEnumerator NoLayerAvailableTest()
	{
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();

		Assert.AreEqual(LayerManager.CurrentLayerNumber, 5);
		Assert.AreEqual(LayerManager.CurrentLayer, testLayer);
	}

	[UnityTest]
	public IEnumerator NextLayerDestructionTest()
	{
		GameObject testObject = new GameObject();
		layerManagerScript.NextLayer();
		yield return new WaitForEndOfFrame();


		Assert.IsTrue(testObject == null);
	}
}
