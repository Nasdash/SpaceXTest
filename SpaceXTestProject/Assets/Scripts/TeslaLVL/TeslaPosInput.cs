using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG.OrbitalElements;
using System;
using UnityEngine.UI;
using System.Globalization;

public class TeslaPosInput : MonoBehaviour
{
    public static List<TeslaPosData> teslaPosDatas = new List<TeslaPosData>();
    public GameObject teslaGameObject;
    public int rowDataNumber = 0; // number CSV row right now
    float counterTime; //Time between increasing data row number
    [Range(0, 50)] public float simulationSpeedUp = 1f;//here we can speed up or slow down Simulation
    [SerializeField]
    float finalSimulationSpeed;
    public float forSmooth;
    DateTime localDateTime;
    public Text infoAboutRoadster;
    float startTime;
    IFormatProvider provider;// for double.TryParse
    NumberStyles style;

    void Start()
    {
        style = NumberStyles.AllowDecimalPoint;// for double.TryParse
        provider = new CultureInfo("en-US");// for double.TryParse

        LoadData();
        CountTimeScale();
        teslaGameObject.transform.position = teslaPosDatas[rowDataNumber].vectorTesla;
        counterTime = Time.time + 1 / finalSimulationSpeed;
    }   

    void FixedUpdate()
    {
        localDateTime = teslaPosDatas[rowDataNumber].dateUTC.ToLocalTime();
        infoAboutRoadster.text = "Time (local): " + localDateTime.ToString()
            + "\nEpoch JD: " + teslaPosDatas[rowDataNumber].epochJD
            + "\nSemi-major axis au: " + teslaPosDatas[rowDataNumber].semimajoraxisAU
            + "\nEccentricity: " + teslaPosDatas[rowDataNumber].eccentricity
            + "\nInclination degrees: " + teslaPosDatas[rowDataNumber].inclinationDegrees
            + "\nLongitude of asc. node degrees: " + teslaPosDatas[rowDataNumber].longitudeofascNodeDegrees
            + "\nArgument of periapsis degrees: "+ teslaPosDatas[rowDataNumber].argumentOfPeriapsisDegrees
            + "\nMean Anomaly degrees: " + teslaPosDatas[rowDataNumber].meanAnomalyDegrees
            + "\nTrue Anomaly degrees: " + teslaPosDatas[rowDataNumber].trueAnomalyDegrees;

        forSmooth += Time.deltaTime * finalSimulationSpeed;
        
        Debug.Log(rowDataNumber);

        if (rowDataNumber == 0)
        {
            teslaGameObject.transform.position = teslaPosDatas[rowDataNumber].vectorTesla;
            PositionSmooth();
        }
        else if (rowDataNumber < teslaPosDatas.Count)
        {
            PositionSmooth();
        }

        //here we are counting when jump to other data line
        if (Time.time >= counterTime && rowDataNumber <= teslaPosDatas.Count)
        {
            rowDataNumber++;
            if (rowDataNumber >= teslaPosDatas.Count)
            {
                rowDataNumber = 0;
            }
            forSmooth = 0;
            CountTimeScale();
            counterTime = Time.time+1/ finalSimulationSpeed;           
        }
        
    }    

    /// <summary>
    /// Smootsh linear transition between two points with Vector3.Lerp
    /// </summary>
    public void PositionSmooth()
    {
        if (rowDataNumber < teslaPosDatas.Count - 1)
        {
            teslaGameObject.transform.position = Vector3.Lerp(teslaPosDatas[rowDataNumber].vectorTesla, teslaPosDatas[rowDataNumber + 1].vectorTesla, forSmooth);
        }
        else
        {
            teslaGameObject.transform.position =teslaPosDatas[rowDataNumber].vectorTesla;
        }
    }

    /// <summary>
    /// Loads data from CSV data
    /// </summary>
    private void LoadData()
    {
        TextAsset roadstePos = Resources.Load<TextAsset>("roadsterpos");
        if (roadstePos == null)
        {
            Debug.Log("Failed to load roadster position data file");
        }
         // Loading CSV file from resoursec folder
        string[] data = roadstePos.text.Split(new char[] { '\n' }); // splitting CSV to rows

        for (int i = 1; i < data.Length - 1; i++) //data.Length - 1 because csv has last row empty
        {
            string[] row = data[i].Split(new char[] { ',' });// splitting rows to data cells
            TeslaPosData t = new TeslaPosData();
            t.epochJD = row[0];
            t.dateUTC = DateTime.ParseExact(row[1], "yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
            if (double.TryParse(row[2], style, provider, out double number2))
            {              
                t.semimajoraxisAU = number2;                
            }
            else
            {
                Debug.Log($"Row {i}, column 2 failed to Parse.");
            }

            if (double.TryParse(row[3], style, provider, out double number3))
            {
                t.eccentricity = number3;
            }
            else
            {
                Debug.Log($"Row {i}, column 3 failed to Parse.");
            }

            if (double.TryParse(row[4], style, provider, out double number4))
            {
                t.inclinationDegrees = number4;
            }
            else
            {
                Debug.Log($"Row {i}, column 4 failed to Parse.");
            }

            if (double.TryParse(row[5], style, provider, out double number5))
            {
                t.longitudeofascNodeDegrees = number5;
            }
            else
            {
                Debug.Log($"Row {i}, column 5 failed to Parse.");
            }

            if (double.TryParse(row[6], style, provider, out double number6))
            {
                t.argumentOfPeriapsisDegrees = number6;
            }
            else
            {
                Debug.Log($"Row {i}, column 6 failed to Parse.");
            }

            if (double.TryParse(row[7], style, provider, out double number7))
            {
                t.meanAnomalyDegrees = number7;
            }
            else
            {
                Debug.Log($"Row {i}, column 7 failed to Parse.");
            }

            if (double.TryParse(row[8], style, provider, out double number8))
            {
                t.trueAnomalyDegrees = number8;
            }
            else
            {
                Debug.Log($"Row {i}, column 8 failed to Parse.");
            }

            teslaPosDatas.Add(t);            
            t.vectorTeslaDouble = Calculations.CalculateOrbitalPosition(t.semimajoraxisAU, t.eccentricity, t.inclinationDegrees, t.longitudeofascNodeDegrees, t.argumentOfPeriapsisDegrees, t.trueAnomalyDegrees);
            t.vectorTesla = new Vector3((Convert.ToSingle(t.vectorTeslaDouble.x) / 10000), (Convert.ToSingle(t.vectorTeslaDouble.y) / 10000), (Convert.ToSingle(t.vectorTeslaDouble.z) / 10000));
        }
    }

    /// <summary>
    /// Calculates what time ratio we need to adjust between two points (24 h per sec = ratio 1)
    /// </summary>
    private void CountTimeScale()
    {
        if (rowDataNumber < teslaPosDatas.Count - 1)
        {
            var d1 = teslaPosDatas[rowDataNumber].dateUTC;
            var d2 = teslaPosDatas[rowDataNumber + 1].dateUTC;
            TimeSpan t = d2 - d1;
            var elapsedDays = t.Days;
            finalSimulationSpeed = simulationSpeedUp / elapsedDays;
        }
        else
        {
            var d11 = teslaPosDatas[rowDataNumber-1].dateUTC;
            var d12 = teslaPosDatas[rowDataNumber].dateUTC;
            TimeSpan ta = d12 - d11;
            var elapsedDaysa = ta.Days;
            finalSimulationSpeed = simulationSpeedUp / elapsedDaysa;
        }
    }
}
