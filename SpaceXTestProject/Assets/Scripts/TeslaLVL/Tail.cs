using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public GameObject scriptContainer;
    public GameObject myLine;
    TeslaPosInput script;
    int row = 1;
    float forSmooth;
    
    LineRenderer lr;

    void Start()
    {
        script = (TeslaPosInput)scriptContainer.GetComponent(typeof(TeslaPosInput));
        if (script == null)
        {
            Debug.Log("Can't load Tesla Position Data for tail rendering");
        }
        row = script.rowDataNumber;
        lr = myLine.GetComponent<LineRenderer>();
    }

    void Update()
    {
        row = script.rowDataNumber;
        forSmooth = script.forSmooth;
        DrawLine();
    }
    /// <summary>
    /// This draws line for previous 20 points, till 20 row it drawns line 1 point till row point
    /// </summary>
    void DrawLine()
    {
        if (row<=1)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, TeslaPosInput.teslaPosDatas[0].vectorTesla);
            lr.SetPosition(1, TeslaPosInput.teslaPosDatas[1].vectorTesla);
        }
        else if(row<=20 && row >1)
        {
            lr.positionCount = row;
            lr.SetPosition(0, TeslaPosInput.teslaPosDatas[row].vectorTesla);            
            for (int i = 2; i < row; i++)
            {
                lr.SetPosition(Math.Abs(i - row), Vector3.Lerp(TeslaPosInput.teslaPosDatas[i - 1].vectorTesla, TeslaPosInput.teslaPosDatas[i].vectorTesla, forSmooth));
            }
            lr.SetPosition(row - 1, TeslaPosInput.teslaPosDatas[0].vectorTesla);
        }
        else if (row > 20 && row<605)
        {
            lr.positionCount = 20;
            for (int i = row-19; i <= row; i++)
            {
                lr.SetPosition(Math.Abs(i-row), Vector3.Lerp(TeslaPosInput.teslaPosDatas[i-1].vectorTesla, TeslaPosInput.teslaPosDatas[i].vectorTesla, forSmooth ));
            }
        }
        else
        {
            lr.positionCount = 1;
            lr.SetPosition(0, TeslaPosInput.teslaPosDatas[row].vectorTesla);
        }
    }
}
