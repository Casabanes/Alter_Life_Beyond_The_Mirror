using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private Vector3 _newPosition;
    private Vector3 _launchPos;
    [SerializeField] private LayerMask _layerToRay;

    void Update()
    {
        _launchPos = transform.parent.position + Vector3.up;
        if (Physics.Raycast(_launchPos, -Vector3.up, out RaycastHit ray, Mathf.Infinity, _layerToRay))
        {
            _newPosition.x = transform.position.x;
            _newPosition.z = transform.position.z;
            _newPosition.y = ray.point.y;
            transform.position = _newPosition;
        }
    }
}
