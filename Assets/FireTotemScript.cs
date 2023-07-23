using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FireTotemScript : MonoBehaviour
{
    public float RotationSpeed = 1.0f;
    private Action _updateDelegate;

    private void Start()
    {
            _updateDelegate += Rotating;
        EventManager.instance.pause += Pause;
    }
    private void FixedUpdate()
    {
        _updateDelegate?.Invoke();
    }
    private void Pause(bool value)
    {
        if (value)
        {
            _updateDelegate = delegate { };
        }
        else
        {
            _updateDelegate = delegate { };
            _updateDelegate += Rotating;
        }
    }
    private void Rotating()
    {
        transform.Rotate(Vector3.up, RotationSpeed);
    }
}
