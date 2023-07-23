using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;
    void Update()
    {
        transform.forward = (_lookAt.position - transform.position).normalized;
    }
}
