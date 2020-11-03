using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneLaunchInfo : MonoBehaviour
{
    [SerializeField]
    private Image launchInfoImage;
    [SerializeField]
    private Text launchName, launchPayload, launchRocketName, launchCounty;
    List<string> shipNames = new List<string>();
    GameObject popupWindowParent;
    GameObject popupWindowCanvas;

    private void Start()
    {
        popupWindowParent = GameObject.Find("ShipPopup");
        if (popupWindowParent == null)
        {
            Debug.LogError("Popup window parent is missing");
        }
        popupWindowCanvas = popupWindowParent.transform.GetChild(0).gameObject;
    }

    public void OnClick()
    {        
        popupWindowCanvas.SetActive(true);
        GameObject.Find("Scripts").GetComponent<ShipPopupWindow>().ActivatePopupWindow(shipNames);
        if (GameObject.Find("Scripts").GetComponent<ShipPopupWindow>() == null)
        {
            Debug.LogError("Can't find script for popup window activation");
        }
    }

    public void SetTextOnButtons(string launchN, int launchP, string launchRocketN, List<string> launchC, bool launchedIf, List<string> shipN)
    {
        string countryText = "";
        List<string> launchCouWithoutDuplicates = launchC.Distinct().ToList();
        for (int i = 0; i < launchCouWithoutDuplicates.Count; i++)
        {
            if (i >0)
            {
                countryText += ", ";
            }
            countryText += launchCouWithoutDuplicates[i];
        }
        launchName.text += launchN;
        launchPayload.text += launchP;
        launchRocketName.text += launchRocketN;
        launchCounty.text += countryText;
        shipNames = shipN;
        if (launchedIf)
        {
            launchInfoImage.color = Color.green;
        }
        else
        {
            launchInfoImage.color = Color.red;
        }
    }

}
