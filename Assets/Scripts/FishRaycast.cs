using System;
using UnityEngine;

public class FishRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    
    public float[] GetDistances()
    {
        var distances = new float[BoidHelper.Directions.Length];
        var directions = BoidHelper.Directions;
        var position = transform.position;
        
        for(var i = 0; i < directions.Length; i++)
        {
            var direction = transform.TransformDirection(directions[i]);
            var ray = new Ray(position, direction);
            
            if (Physics.Raycast(ray, out var hit, 5f, layerMask))
            {
                Debug.DrawRay(position, direction * hit.distance, Color.yellow);
                distances[i] = 1f / hit.distance;
            }
            else
            {
                Debug.DrawRay(position, direction * 5f, Color.white);
                distances[i] = 0;
            }
        }
        
        return distances;
    }

    private void Update()
    {
        GetDistances();
    }
}
