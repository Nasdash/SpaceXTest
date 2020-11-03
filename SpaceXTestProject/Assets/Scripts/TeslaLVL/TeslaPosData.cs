using RG.OrbitalElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaPosData
{
    /// <summary>
    /// Creates objects to store data from Roadster positions CSV
    /// </summary>
    public TeslaPosData()
    {

    }
    public string epochJD;
    public DateTime dateUTC;
    public double semimajoraxisAU;
    public double eccentricity;
    public double inclinationDegrees;
    public double longitudeofascNodeDegrees;
    public double argumentOfPeriapsisDegrees;
    public double meanAnomalyDegrees;
    public double trueAnomalyDegrees;
    public Vector3Double vectorTeslaDouble;
    public Vector3 vectorTesla;
}
