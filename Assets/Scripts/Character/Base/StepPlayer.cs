using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Destroy(gameObject);
            }
        }    
    }
    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Stop();
        _audioSource.clip = _clips[Random.Range(0, _clips.Length)];
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.Play();
    }
}
