using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetRotateMouse : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    
    private float _xRotation;
    private float _yRotation;

    private void Update()
    {
        // Rotate using mouse drag
        if (Input.GetMouseButton(0))
        {
            var x = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            var y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            _yRotation += y;
            _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
            _xRotation += x;
            
            transform.localRotation = Quaternion.Euler(-_yRotation, _xRotation, 0f);
        }
        
        // Press R to reset
        if (!Input.GetKeyDown(KeyCode.R)) return;
        transform.rotation = Quaternion.identity;
        _xRotation = 0;
        _yRotation = 0;
    }
}
