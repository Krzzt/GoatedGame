using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
   [SerializeField] private GameObject charSelectObject;


    private void Awake()
    {
        charSelectObject.SetActive(false);
    }
    public void LoadCharSelect()
    {
        charSelectObject.SetActive(true);
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
