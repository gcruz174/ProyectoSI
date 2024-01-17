using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidHelper
{
    private const int NumViewDirections = 500;
    public static readonly Vector3[] Directions;

    static BoidHelper () 
    {
        const int actualNumViewDirections = NumViewDirections / 3 * 2;
        Directions = new Vector3[actualNumViewDirections];
        
        var goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        var angleIncrement = Mathf.PI * 2 * goldenRatio;
        
        // Generate directions only on the front hemisphere
        for (var i = 0; i < actualNumViewDirections; i++)
        {
            var t = (float) i / NumViewDirections;
            var inclination = Mathf.Acos(1 - 2 * t);
            var azimuth = angleIncrement * i;
            
            var x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            var y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            var z = Mathf.Cos(inclination);
            
            Directions[i] = new Vector3(x, y, z);
        }

    }

}