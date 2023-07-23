using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWhenHitted : MonoBehaviour, IHittable
{
    [SerializeField] private float _forceToApply;
    [SerializeField] private ForceMode _forceModeToApply;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;

    public void GetHit()
    {
        if (_audioSource == null)
        {
            return;
        }
        if (!_audioSource.isPlaying)
        {
            _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
            _audioSource.Play();
        }
        if (_rigidBody != null)
        {
            _rigidBody.AddForce(transform.forward * _forceToApply, _forceModeToApply);
        }
    }
}
