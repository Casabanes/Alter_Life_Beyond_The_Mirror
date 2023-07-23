using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrowerLife : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject _normalMesh;
    [SerializeField] private GameObject _hitedMesh;
    [SerializeField] private float _timeToRecovery;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _fireDamageObject;
    [SerializeField] private FireTotemScript _fireTotemScript;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private int _timeToDestroy;
    [SerializeField] private float _longTimeToRecovery;
    [SerializeField] private AudioSource _damageAs;

    private int _hittedTimes = 0;
    public void GetHit()
    {
        _hittedTimes++;
        _normalMesh.SetActive(false);
        _hitedMesh.SetActive(true);
        _particleSystem.Stop();
        _fireTotemScript.enabled = false;
        _fireDamageObject.SetActive(false);
        _audioController.EndingFire();
        _damageAs.Stop();
        _damageAs.Play();
        if (_hittedTimes < _timeToDestroy)
        {
            StartCoroutine(Recovery(_timeToRecovery));
        }
        else
        {
            StartCoroutine(Recovery(_longTimeToRecovery));
        }
    }
    private IEnumerator Recovery(float time)
    {
        yield return new WaitForSeconds(time);
        _particleSystem.Play();
        _audioController.StartFire();
        _fireTotemScript.enabled = true;
        _fireDamageObject.SetActive(true);
        _hitedMesh.SetActive(false);
        _normalMesh.SetActive(true);
    }
}
