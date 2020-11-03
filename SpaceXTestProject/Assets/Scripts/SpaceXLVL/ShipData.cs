using RG.OrbitalElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipData
{
    /// <summary>
    /// Extract data from API to class
    /// </summary>
    public ShipData()
    {

    }
    public string Name { get; set; }
    public string ID { get; set; }
    public string MissionsNames { get; set; }
    public string Type { get; set; }
    public string Homeport { get; set; }
    public string ImageURL { get; set; }
    public int MissionsCount { get; set; }
}
