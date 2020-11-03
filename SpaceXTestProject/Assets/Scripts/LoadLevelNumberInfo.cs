using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelNumberInfo: MonoBehaviour
{
    public static string levelToLoad;
    public static bool loadMainMenu = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
