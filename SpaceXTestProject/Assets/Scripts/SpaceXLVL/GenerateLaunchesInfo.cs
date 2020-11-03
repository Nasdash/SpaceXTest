using System.Collections;
using System.Collections.Generic;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Net;
using System.Linq;
using UnityEngine.UI;
using Packages.Rider.Editor.UnitTesting;
using Newtonsoft.Json.Linq;

public class GenerateLaunchesInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject launchInfoPrefab;
    [SerializeField]
    private GameObject parentObject;
    bool loaded = false;
    public Text loadingInfo;
    List<LaunchData> launchDatas;

    // Start is called before the first frame update
    void Start()
    {
        launchDatas = new List<LaunchData>();
        StartCoroutine(GetText());
    }

    // Update is called once per frame
    void Update()
    {
        if (!loaded)
        {
            loadingInfo.text = "Loading...";
        }
        else
        {
            loadingInfo.enabled = false;
        }
    }

    IEnumerator GetText()
    {
        UnityWebRequest flightWEB = UnityWebRequest.Get("https://api.spacexdata.com/v3/launches");
        yield return flightWEB.SendWebRequest();

        if (flightWEB.isNetworkError || flightWEB.isHttpError)
        {
            Debug.Log(flightWEB.error);
        }
        else
        {
            JArray launchInfo = JArray.Parse(flightWEB.downloadHandler.text);
            if (launchInfo == null)
            {
                Debug.Log("Failed to parse launch info to JArray");
            }
            for (int i = 0; i < launchInfo.Count; i++)
            {
                launchDatas.Add(new LaunchData());
                GameObject lauchButoon = Instantiate(launchInfoPrefab) as GameObject;
                lauchButoon.transform.SetParent(parentObject.transform);
                if (launchInfo[i]["rocket"]["second_stage"]["payloads"]!=null)
                {
                    JToken pn = launchInfo[i]["rocket"]["second_stage"]["payloads"];
                    launchDatas[i].PayloadNumber = pn.Count<JToken>();

                    for (int k = 0; k < launchDatas[i].PayloadNumber; k++)
                    {
                        if (launchInfo[i]["rocket"]["second_stage"]["payloads"][k]["nationality"] != null)
                        {
                            launchDatas[i].Countries.Add(launchInfo[i]["rocket"]["second_stage"]["payloads"][k]["nationality"].ToString());
                        }
                        else
                        {
                            Debug.Log($"No country value for mission Nr {i}, payload {k}");
                        }
                    }

                }
                else
                {
                    Debug.Log($"Payload count for mission Nr {i} isn't available and set to 0, countires list is empty");
                    launchDatas[i].PayloadNumber = 0;
                }

                if (launchInfo[i]["mission_name"] != null)
                {
                    launchDatas[i].Missions = launchInfo[i]["mission_name"].ToString();
                }
                else
                {
                    Debug.Log($"Mission name for mission Nr {i} isn't available and set to empty");
                    launchDatas[i].Missions = "";
                }

                if (launchInfo[i]["rocket"]["rocket_name"] != null)
                {
                    launchDatas[i].RocketName = launchInfo[i]["rocket"]["rocket_name"].ToString();
                }
                else
                {
                    Debug.Log($"Rocket name for mission Nr {i} isn't available and set to empty");
                    launchDatas[i].RocketName = "";
                }

                if (launchInfo[i]["upcoming"] != null)
                {
                    launchDatas[i].Upcoming = !launchInfo[i]["upcoming"].Value<bool>();
                }
                else
                {
                    Debug.Log($"Mission Nr {i} status isn't available and set to upcoming");
                    launchDatas[i].Upcoming = true;
                }

                if (launchInfo[i]["ships"]!= null)
                {
                    for (int m = 0; m < launchInfo[i]["ships"].Count(); m++)
                    {
                        launchDatas[i].Ships.Add(launchInfo[i]["ships"][m].ToString());
                    }
                }
                else
                {
                    Debug.Log($"Mission Nr {i} ships list isn't available");
                }

                lauchButoon.GetComponent<OneLaunchInfo>().SetTextOnButtons(launchDatas[i].Missions, launchDatas[i].PayloadNumber, launchDatas[i].RocketName, launchDatas[i].Countries, launchDatas[i].Upcoming, launchDatas[i].Ships);
            }
        }
        loaded = true;
    }
}
