using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public Text textToChange;

    private void Start()
    {
        textToChange.text = "Loading...";
        LoadMainMenu();
    }
    public void LoadMainMenu()
    {        
        SceneManager.LoadScene("MainMenu");
    }
}
