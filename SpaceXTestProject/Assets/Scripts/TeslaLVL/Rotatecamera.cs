using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatecamera : MonoBehaviour
{
    //works only with touch input
    private Vector3 firstpoint; 
    private Vector3 secondpoint;
    private float xAngle  = 0.0f;
    private float yAngle  = 0.0f;
    private float xAngTemp  = 0.0f;
    private float yAngTemp  = 0.0f;
    void Start()
    {

    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
                transform.parent.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }
        }
    }

    public void ResetCamera()
    {
        transform.parent.rotation = Quaternion.Euler(0f, 0f, 0.0f);
    }
}
