using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void LoadScene(string a)
    {
        LoadLevelNumberInfo.levelToLoad = a;
        SceneManager.LoadScene("LoadingScreen");        
    }

    public void MainMenuLoading(bool b)
    {
        LoadLevelNumberInfo.loadMainMenu = b;
    }
}
