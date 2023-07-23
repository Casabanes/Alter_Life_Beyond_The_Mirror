using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _fireStart;
    [SerializeField] private AudioClip _fireStay;
    [SerializeField] private AudioClip _fireEnd;
    [SerializeField] private Action _updateDelegate;
    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Destroy(gameObject);
            }
        }
        _audioSource.loop = false;
        _audioSource.clip = _fireStart;
        StartFire();
    }
    private void Update()
    {
        _updateDelegate?.Invoke();
    }
    public void StartFire()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = _fireStart;
        _audioSource.Play();
        _updateDelegate += StartingFire;
    }
    public void StartingFire()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _fireStay;
            _updateDelegate -= StartingFire;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
    public void EndingFire()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = _fireEnd;
        _audioSource.Play();
    }
}
