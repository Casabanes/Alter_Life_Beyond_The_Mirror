using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCheckPoint : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _launchPosition;
    private Vector3 _newPosition;
    [SerializeField] private LayerMask _layerFloor;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    void Start()
    {
        if(_audioClip && _audioSource)
        {
        _audioSource.clip = _audioClip;
        }
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerModel>().gameObject.transform;
        }
    }
    void Update()
    {
        CheckIfPlaceIsSafe();
    }
    private void CheckIfPlaceIsSafe()
    {
        _launchPosition = _player.transform.position + Vector3.up;
        if (Physics.Raycast(_launchPosition, -Vector3.up,out RaycastHit ray,Mathf.Infinity, _layerFloor))
        {
            _newPosition = _player.transform.position;
            _newPosition.y = ray.point.y;
            transform.position = _newPosition;
            transform.forward = _player.transform.forward;
        }
        else
        {
            transform.position = _newPosition - transform.forward;
        }
    }
    public void SoundSplash()
    {
            _audioSource.Stop();
            _audioSource.Play();
    }
}
