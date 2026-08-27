using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
   [field: SerializeField] public GameObject CharSelectObject {  get; set; }


    private void Awake()
    {
        CharSelectObject.SetActive(false);
    }
    public void LoadCharSelect()
    {
        CharSelectObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LoadOptions()
    {
        //nothing yet
    }

    public void Quit()
    {
        Application.Quit();
    }
}
