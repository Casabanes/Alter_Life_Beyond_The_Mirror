using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraPause : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _camera;

    void Start()
    {
        if (_camera == null)
        {
            _camera = FindObjectOfType<CinemachineFreeLook>();
            if (_camera == null)
            {
                Destroy(gameObject);
            }
        }
        EventManager.instance.pause += OnOffCamera;
    }
    private void OnOffCamera(bool value)
    {
        if (value)
        {
            _camera.m_XAxis.m_MaxSpeed= 0;
            _camera.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            _camera.m_XAxis.m_MaxSpeed = 300;
            _camera.m_YAxis.m_MaxSpeed = 2;
        }
    }
}
