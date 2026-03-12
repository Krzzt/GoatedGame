using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class SaveManager
{
    public static SaveState currentSave = new SaveState();
    public static Action SavingGame;
    public static Action LoadingGame;

    public static bool CheckIfFileExists() //also creates file if necessary
    {
        if (!Directory.Exists(Application.dataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.dataPath + "/saves");
        }

        if (!File.Exists(Application.dataPath + "/saves/saveFile.json"))
        {
            FileStream fs = File.Create(Application.dataPath + "/saves/saveFile.json");
            fs.Close();
            return false;
        }
        return true;
    }

    public static IEnumerator SaveGame() //as an IEnumerator to wait for the end of the frame (so everything can save their stuff in currentSave)
    {
        //still need to set everything in currentSave
        SavingGame?.Invoke();
        yield return new WaitForEndOfFrame();
        CheckIfFileExists(); //also creates file if necessary
        string savedGameString = JsonUtility.ToJson(currentSave, true); //pretty print :)
        File.WriteAllText(Application.dataPath + "/saves/saveFile.json", savedGameString);
    }

    public static void LoadGame()
    {
        if (CheckIfFileExists())
        {
            string loadGameString = File.ReadAllText(Application.dataPath + "/saves/saveFile.json");
            JsonUtility.FromJsonOverwrite(loadGameString, currentSave);
        }
        else
        {
            currentSave = new SaveState();
        }
        LoadingGame?.Invoke();
        //still need to Load everything in from currentSave
    }
}
