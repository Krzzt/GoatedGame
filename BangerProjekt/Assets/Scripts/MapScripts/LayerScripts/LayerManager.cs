using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayerManager : MonoBehaviour
{
    [field:SerializeField] public static Layer CurrentLayer {get; set;}
    [field: SerializeField] public static int CurrentLayerNumber { get; set; } = 0;
    private AllLayers AllLayerScript;

    private void Awake()
    {
        AllLayerScript = gameObject.GetComponent<AllLayers>(); //Both is in the LayerManager
        NextLayer();
    }

    public void NextLayer()
    {
        CurrentLayerNumber++;
        List<Layer> possibleLayers = new List<Layer>();
        foreach (Layer layer in AllLayerScript.Layers) //check every Layer
        {
            foreach (int i in layer.PossibleLayers) //check every possible LayerNumber in every Layer
            {
                //idk man probably could be optimised because the int i in layer.PossibleLayers is ascending so if ur above the number it couild stop
                //but i dont wanna do it rn because small optimisation but big work for me
                if (i == CurrentLayerNumber)
                {
                    possibleLayers.Add(layer); //if they match, this layer could generate
                }
            }
        }
        if (possibleLayers.Count > 0)
        {
            CurrentLayer = possibleLayers[Random.Range(0, possibleLayers.Count)]; //choose a random one from the possible ones
        }
        else
        {
            CurrentLayer = AllLayerScript.Layers[0]; //if nothing is found, default to the first in the allLayerScript
        }


    }

    public static List<GameObject> GetEnemyListFromLayer()
    {
        return CurrentLayer.SpawnableEnemies;

    }
}
