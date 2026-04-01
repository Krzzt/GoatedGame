using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    private SaveState newSaveState;
    [SerializeField] private Class classSelected;
    [SerializeField] private int seedSelected;
    [field: SerializeField] public List<Class> AllClasses { get; set; }

    private InputField seedInput;

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private TMP_Text selectedText;
    
    private void OnEnable()
    {
        SaveManager.SavingGame += SaveAndStart;
    }

    private void OnDisable()
    {
        SaveManager.SavingGame -= SaveAndStart;
    }

    private void Awake()
    {
        seedInput = GameObject.Find("SeedInput").GetComponent<InputField>();  //set by name AAAAAAAAAAA
        SelectClass(AllClasses[0]); //placeholder for safety
    }
    public void SelectClass(Class classToPick)
    {
        classSelected = classToPick;
        newSaveState = new SaveState(classSelected);
        selectedText.SetText("Selected Class: " + classSelected.Name);
    }

    public void SetSeed(string input)
    {
        seedSelected = int.Parse(input);
        Debug.Log("Seed: " + seedSelected);
    }

    public void SaveAndStart()
    {
        SaveManager.currentSave = newSaveState;
        StartCoroutine(WaitAndLoadGame());
    }

    public IEnumerator WaitAndLoadGame()
    {
        yield return new WaitForNextFrameUnit(); //so after the save shit has happened
        SceneManager.LoadScene("Game");
    }
    public void StartGame()
    {
        if (!classSelected)
        {
            classSelected = AllClasses[0]; // if no class selected, just take the first one (placeholder ig)
        }
        newSaveState.PlayerClass = classSelected;
        if (seedSelected != 0)
        {
            newSaveState.Seed = seedSelected;
            newSaveState.IsSeeded = true;
        }
        StartCoroutine(SaveManager.SaveGame());


    }

    public void LoadTitleScreen()
    {
        titleScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
