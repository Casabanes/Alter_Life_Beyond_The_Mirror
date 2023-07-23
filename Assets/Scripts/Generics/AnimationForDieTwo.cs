using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationForDieTwo : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _floatSpeed;
    [SerializeField] private float _minHigh;
    [SerializeField] private float _maxHigh;
    [SerializeField] private bool _isGoingUp = true;
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;

        if (_isGoingUp)
        {
            if (transform.position.y >= _maxHigh)
            {
                _isGoingUp = false;
                _floatSpeed *= -1;
                _rotationSpeed *= -1;
            }
        }
        else
        {
            if(transform.position.y<= _minHigh)
            {
                _isGoingUp = true;
                _floatSpeed *= -1;
                _rotationSpeed *= -1;
            }
        }

    }
}
