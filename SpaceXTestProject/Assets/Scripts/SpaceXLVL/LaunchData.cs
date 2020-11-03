using RG.OrbitalElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchData
{
    /// <summary>
    /// Extract data from API to class for one mission
    /// </summary>
    public LaunchData()
    {
        Countries = new List<string>();
        Ships = new List<string>();
    }
    public string Missions { get; set; }
    public string RocketName { get; set; }
    public int PayloadNumber { get; set; }
    public bool Upcoming { get; set; }
    public List<string> Countries { get; set; }
    public List<string> Ships { get; set; }
}
