using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private float enginePressure = 100;
    private float maxEnginePressure = 110, minEnginepressure = 90;
    public float CalculateLowPressure
    {
        get
        {
            if (enginePressure < minEnginepressure)
                return minEnginepressure - enginePressure;
            else
                return 0;
        }
    }

}