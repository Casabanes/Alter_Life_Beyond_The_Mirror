using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSword : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _metalSound;
    [SerializeField] private AudioClip _woodSound;
    [SerializeField] private AudioClip _strawSound;
    [SerializeField] private AudioClip _meatSound;
    [SerializeField] private float _minPitch=0.8f;
    [SerializeField] private float _maxPitch=1.2f;

    public GameObject efecto;

    private bool _canPlay = true;

    private void Start()
    {
        if (!_audioSource)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_canPlay)
        {
            /*
            SoundWhenHitted hittedThing = other.GetComponent<SoundWhenHitted>();
            if (hittedThing != null)
            {
                switch (hittedThing.type)
                {
                    case SoundWhenHitted.SoundType.Meat:
                        _audioSource.clip = _meatSound;
                        break;
                    case SoundWhenHitted.SoundType.Metal:
                        _audioSource.clip = _metalSound;
                        break;
                    case SoundWhenHitted.SoundType.Wood:
                        _audioSource.clip = _woodSound;
                        break;
                    case SoundWhenHitted.SoundType.Straw:
                        _audioSource.clip = _strawSound;
                        break;
                }
            }
            _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
            _audioSource.Play();*/
            if (efecto != null)
            {
                Instantiate(efecto, gameObject.transform.position, Quaternion.identity);
            }
            _canPlay = false;
            StartCoroutine(CoolDown());
        }
    }
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        _canPlay= true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
}
