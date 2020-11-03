using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelScript : MonoBehaviour
{
    AsyncOperation asyncOp;

    public Slider progressBar;
    public Text loadingText;
    public Button continueButton;

    public bool sceneLoaded;

    void Start()
    {
        LoadLevelLoadingScreen();
        continueButton.gameObject.SetActive(false);
        sceneLoaded = false;
    }

    void Update()
    {

    }
    public void LoadLevelLoadingScreen()
    {
        loadingText.gameObject.SetActive(true);
        loadingText.text = "Loading...";
        StartCoroutine(LoadLevelWithRealProgress());

    }

    IEnumerator LoadLevelWithRealProgress()
    {
        asyncOp = SceneManager.LoadSceneAsync(LoadLevelNumberInfo.levelToLoad);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            if (asyncOp.progress <= 0.1f)
            {
                progressBar.value = 0f;
            }

            else if (asyncOp.progress < 0.9f && asyncOp.progress > 0.1f)
            {
                progressBar.value = asyncOp.progress - 0.1f;
            }

            else if (asyncOp.progress == 0.9f && sceneLoaded == false)
            {
                
                sceneLoaded = true;
                progressBar.value = 0.95f;                
                yield return new WaitForSeconds(0.1f);
                progressBar.value = 1f;
                loadingText.gameObject.SetActive(false);
                if (!LoadLevelNumberInfo.loadMainMenu)
                {
                    continueButton.gameObject.SetActive(true);
                }
                else
                {
                    LoadLevelNumberInfo.loadMainMenu = false;
                    asyncOp.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void ContinueGame()
    {
        asyncOp.allowSceneActivation = true;
    }
}
