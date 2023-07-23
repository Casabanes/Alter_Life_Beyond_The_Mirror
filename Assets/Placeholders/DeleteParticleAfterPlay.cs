using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticleAfterPlay : MonoBehaviour
{
    //[SerializeField] private ParticleSystem _ps;
    [SerializeField] private AudioSource _as;
    [SerializeField] public AudioClip _clip;
    [SerializeField] private ParticleSystem _ps;

    private void Start()
    {
       if (_ps == null)
        {
            _ps = GetComponent<ParticleSystem>();
        }
        if (_as == null)
        {
            _as = GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        if (_ps != null)
        {
            if (!_ps.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
    public void PlayDaSound(AudioClip ac)
    {
        if (_as == null)
        {
            _as = GetComponent<AudioSource>();
            if (_as == null)
            {
                return;
            }
        }
        _as.Stop();
        _clip = ac;
        _as.Play();
    }
}
