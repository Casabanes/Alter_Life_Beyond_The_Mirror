using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _objectToFollow;
    [SerializeField] private Vector3 _offset;
    void Update()
    {
        if (_objectToFollow == null)
        {
            return;
        }
        transform.position = _objectToFollow.position + _offset;
    }
    public void SetTarget(Transform t)
    {
        _objectToFollow = t;
    }
}
