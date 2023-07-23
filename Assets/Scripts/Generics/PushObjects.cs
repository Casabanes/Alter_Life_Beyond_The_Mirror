using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    [SerializeField] private int _layerToPush;
    [SerializeField] private float _forceToApply;
    [SerializeField] private ForceMode _forceModeToApply;
    [SerializeField] private Transform _transform;
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.layer == _layerToPush)
        {
            Rigidbody target = other.gameObject.GetComponent<Rigidbody>();
            if (target != null)
            {
                target.AddForce(_transform.forward * _forceToApply, _forceModeToApply);
            }
        }*/
    }
}
