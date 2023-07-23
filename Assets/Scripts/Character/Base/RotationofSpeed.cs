using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationofSpeed : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    private float _time;
    [Range(0.0f, 1.0f)]
    private float _timeValue;
    [SerializeField] private float _timeToInterpolate = 0.015f;
    [SerializeField] private float _timeToInterpolateWhenIsAttacking = 0.03f;
    private float _originalTimeToInterpolate;
    private float _realTimeToInterpolate;

    private Vector3 _lookAtDirection;
    private const int _constZero=0;
    private void Start()
    {
        _originalTimeToInterpolate = _timeToInterpolate;
    }
    void Update()
    {
            _lookAtDirection.y = _constZero;
        if (_lookAtDirection == Vector3.zero) 
        {
            _time = _constZero;
            _timeValue = _constZero;
            return;
        }
        RealTimeToInterpolate();
        if (_time < _realTimeToInterpolate)
        {
            _time += Time.deltaTime;
            Mathf.Clamp(_time, _constZero, _realTimeToInterpolate);
            Mathf.Clamp(_timeValue, _constZero, _realTimeToInterpolate);
            _timeValue = _time / _realTimeToInterpolate;
            transform.forward = Vector3.Slerp(transform.forward, _lookAtDirection, _timeValue);
        }
        else
        {
            _time      = _constZero;
            _timeValue = _constZero;
        }
    }
    public void DeclarationOfDirection(float x, float z)
    {
        _lookAtDirection.x = x;
        _lookAtDirection.z = z;
        _lookAtDirection.Normalize();
    }
    private void RealTimeToInterpolate()
    {
        _realTimeToInterpolate = _timeToInterpolate * Vector3.Angle(_lookAtDirection, transform.forward);
    }
    public void TimeToInterpolateIsLowOrNot(bool value)
    {
        if (value)
        {
            _timeToInterpolate = _timeToInterpolateWhenIsAttacking;
        }
        else
        {
            _timeToInterpolate = _originalTimeToInterpolate;
        }

    }
}
