using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupWindow : MonoBehaviour
{
    [SerializeField]
    GameObject popUpWindow;
    public void CloseWindow()
    {
        popUpWindow.SetActive(false);

        ObjectPooler.SharedInstance.ReturnToPool();
    }
}
