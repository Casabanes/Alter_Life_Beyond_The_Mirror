using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolinoGirador : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    void Update()
    {
        transform.Rotate(Vector3.right * _rotationSpeed * Time.deltaTime);
    }
}
