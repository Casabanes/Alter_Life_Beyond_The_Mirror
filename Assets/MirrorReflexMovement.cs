using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflexMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private Transform _mirror;
    [SerializeField] private float _distanceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPlayer = _mirror.InverseTransformPoint(_playerTarget.position);
        transform.position = _mirror.TransformPoint(new Vector3(localPlayer.x* _distanceMultiplier, localPlayer.y* _distanceMultiplier, -localPlayer.z* _distanceMultiplier));

        Vector3 lookatmirror = _mirror.TransformPoint(new Vector3(-localPlayer.x, localPlayer.y, localPlayer.z));
        transform.LookAt(lookatmirror);
    }
}
