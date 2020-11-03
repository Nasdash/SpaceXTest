using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SimpleJSON;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;

public class ShipPopupWindow : MonoBehaviour
{  
    List<string> shipName;

    //JSONNode shipInfo;
    JArray shipInfoNew;
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private GameObject shipInfoPrefab;
    [SerializeField]
    private GameObject windPopup;
    List<ShipData> shipsData;

    void Start()
    {
        shipsData = new List<ShipData>();
        windPopup.SetActive(false);
        StartCoroutine(GetText());
    }

    /// <summary>
    /// Activated popup windows with info about ships involved in mission
    /// </summary>
    /// <param name="shipN">All ship names involved in mission</param>
    public void ActivatePopupWindow(List<string> shipN)
    {
        gameObject.SetActive(true);
        shipName = shipN;
        if (shipName.Count == 0)//checking if there is any ship involved in mission
        {
            //taking object from pool
            GameObject shipButoon = ObjectPooler.SharedInstance.GetPooledObject();
            if (shipButoon == null)
            {
                Debug.Log("GameObject shipButton missed");
            }
            shipButoon.transform.SetParent(parentObject.transform);
            shipButoon.SetActive(true);
            shipButoon.GetComponent<OneShipInfo>().NoShips();
        }
        else
        {
            for (int i = 0; i < shipInfoNew.Count; i++)
            {               
                bool check = shipName.Contains(shipsData[i].ID);
                if (check)
                {
                    GameObject shipButoon = ObjectPooler.SharedInstance.GetPooledObject();
                    if (shipButoon == null)
                    {
                        Debug.Log("GameObject shipButton missed");
                    }
                    
                    shipButoon.transform.SetParent(parentObject.transform);
                    shipButoon.SetActive(true); 
                    shipButoon.GetComponent<OneShipInfo>().SetTextOnButtons(shipsData[i]);
                }
            }
        }
    }

    /// <summary>
    /// Loading JSON file
    /// </summary>
    /// <returns></returns>
    IEnumerator GetText()
    {
        UnityWebRequest flightWEB = UnityWebRequest.Get("https://api.spacexdata.com/v3/ships");
        yield return flightWEB.SendWebRequest();

        if (flightWEB.isNetworkError || flightWEB.isHttpError)
        {
            Debug.Log(flightWEB.error);
        }
        else
        {
            shipInfoNew = JArray.Parse(flightWEB.downloadHandler.text);
            for (int i = 0; i < shipInfoNew.Count; i++)
            {
                shipsData.Add(new ShipData());

                if (shipInfoNew[i]["ship_name"] != null)
                {
                    shipsData[i].Name = shipInfoNew[i]["ship_name"].ToString();
                }
                else
                {
                    Debug.Log($"Name for ship Nr {i} isn't available and set to empty");
                    shipsData[i].Name = "";
                }
                
                if (shipInfoNew[i]["ship_id"] != null)
                {
                    shipsData[i].ID = shipInfoNew[i]["ship_id"].ToString();
                }
                else
                {
                    Debug.Log($"ID for ship Nr {i} isn't available and set to empty");
                    shipsData[i].ID = "";
                }

                if (shipInfoNew[i]["missions"] != null)
                {
                    shipsData[i].MissionsCount = shipInfoNew[i]["missions"].Count();

                    JToken shipMissions = shipInfoNew[i]["missions"];
                    for (int m = 0; m < shipMissions.Count<JToken>(); m++)
                    {
                        shipsData[i].MissionsNames += shipMissions[m]["name"] + ", ";
                    }
                }
                else
                {
                    Debug.Log($"Can't get missions count for ship Nr {i}");
                    shipsData[i].MissionsCount = 0;
                    shipsData[i].MissionsNames = "";
                }

                if (shipInfoNew[i]["ship_type"] != null)
                {
                    shipsData[i].Type = shipInfoNew[i]["ship_type"].ToString();
                }
                else
                {
                    Debug.Log($"Type for ship Nr {i} isn't available and set to empty");
                    shipsData[i].Type = "";
                }
                
                if (shipInfoNew[i]["home_port"] != null)
                {
                    shipsData[i].Homeport = shipInfoNew[i]["home_port"].ToString();
                }
                else
                {
                    Debug.Log($"Homeport for ship Nr {i} isn't available and set to empty");
                    shipsData[i].Homeport = "";
                }
                //
                if (shipInfoNew[i]["image"] != null)
                {
                    shipsData[i].ImageURL = shipInfoNew[i]["image"].ToString();
                }
                else
                {
                    Debug.Log($"Image for ship Nr {i} isn't available and set to empty");
                    shipsData[i].ImageURL = "";
                }

            }
        }
    }
}
