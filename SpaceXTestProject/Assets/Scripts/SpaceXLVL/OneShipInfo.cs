using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneShipInfo : MonoBehaviour
{
    [SerializeField]
    private Text shipName, shipType, shipHomeport, missionsName, shipMissionsNumber;

    string shipImageURL;
    List<string> shipNames = new List<string>();
    [SerializeField]
    GameObject popupWindowScript;
    GameObject popupWindowCanvas;

    private void Start()
    {
        popupWindowScript = GameObject.Find("ShipPopup");
        if (popupWindowScript == null)
        {
            Debug.Log("Popup window script is missing");
        }
    }
    /// <summary>
    /// Opens ship image
    /// </summary>
    public void OnClick()
    {
        if (shipImageURL != "")
        Application.OpenURL(shipImageURL);
    }
    /// <summary>
    /// sets text on initialized button
    /// </summary>
    /// <param name="shipN"> Ship name</param>
    /// <param name="shipNumberM">Number of missions ship was involved</param>
    /// <param name="missionsN">All missions name</param>
    /// <param name="shipT">Ship type</param>
    /// <param name="homeport">Ship homeport</param>
    /// <param name="imageU">Ship image URL, will open in browser</param>
    public void SetTextOnButtons(ShipData shipA)
    {
        
        ShipData ship = shipA;
        shipName.text = ship.Name;
        shipType.text = "Ship type: "+ ship.Type;
        shipHomeport.text = "Homeport: "  + ship.Homeport;
        shipImageURL = ship.ImageURL;
        missionsName.text = "Missions names: "+ ship.MissionsNames;
        shipMissionsNumber.text = "Number of missions: "+ ship.MissionsCount;
    }
    /// <summary>
    /// If missions doesn't have ships
    /// </summary>
    public void NoShips()
    {
        shipName.text = "";
        shipType.text = "";
        shipHomeport.text = "";
        shipImageURL = "";
        missionsName.text = "No ships used in launch";
        shipMissionsNumber.text = "";
    }
}
