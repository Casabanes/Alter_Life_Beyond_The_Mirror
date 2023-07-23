using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MirrorPiece : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _mirror;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private int _mirrorLayer;
    [SerializeField] private float _speed;
    [SerializeField] private float _magnetRadius;
    [SerializeField] private GameObject _effect;


    [SerializeField] private float _floatingMinimumAmplitude;
    [SerializeField] private float _floatingAmplitude;
    [SerializeField] private float _floatingMaximumAmplitude;

    [SerializeField] private float _floatingSpeed;
    [SerializeField] private float _minFloatingSpeed;
    [SerializeField] private float _maxFloatingSpeed;

    [SerializeField] private float _floatingAmplitudeXZ;
    [SerializeField] private float _minFloatingAmplitudeXZ;
    [SerializeField] private float _maxFloatingAmplitudeXZ;

    [SerializeField] private float _floatingSpeedXZ;
    [SerializeField] private float _minFloatingSpeedXZ;
    [SerializeField] private float _maxFloatingSpeedXZ;

    [SerializeField] private float _timeToGoUp;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minRotateSpeed;
    [SerializeField] private float _maxRotateSpeed;
    private Action _mode;
    private void Start()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        _player = GameManager.instance.GetPlayerObjetct().transform;
        StartCoroutine(ActivateMagnetEffect());
        _mirror = GameManager.instance.mirror.gameObject;
        _floatingAmplitude = UnityEngine.Random.Range(_floatingMinimumAmplitude, _floatingMaximumAmplitude);
        _floatingSpeed = UnityEngine.Random.Range(_minFloatingSpeed, _maxFloatingSpeed);
        _floatingSpeedXZ = UnityEngine.Random.Range(_minFloatingSpeedXZ, _maxFloatingSpeedXZ);
        _rotateSpeed = UnityEngine.Random.Range(_minRotateSpeed, _maxRotateSpeed);

        if (UnityEngine.Random.value > 0.5f)
        {
            _rotateSpeed *=-1;
        }
    }

    private void FixedUpdate()
    {
        _mode?.Invoke();
    }
    public void CheckingIfCanFloat()
    {
        if (_rigidBody.velocity == Vector3.zero)
        {
            _rigidBody.useGravity = false;
            _mode -= CheckingIfCanFloat;
            _mode += Rotating;
            StartCoroutine(GoingUp());
        }
    }
    public void MagnetEffect()
    {
        if(Vector3.Distance(_player.transform.position, transform.position) <= _magnetRadius
            || Vector3.Distance(_mirror.transform.position, transform.position) <= _magnetRadius)
        {
            _mode = Magnet;
        }
        else
        {
            if(_mode == Magnet)
            {
                _mode -= Magnet;
                _rigidBody.useGravity = true;
                StartCoroutine(ActivateMagnetEffect());
            }
        }
    }
    private void Magnet()
    {
        Vector3 dir = (_mirror.transform.position - transform.position).normalized;
        transform.position += dir * _speed * Time.deltaTime;
    }
    IEnumerator ActivateMagnetEffect()
    {
        yield return new WaitForSeconds(0.7f);
        _mode += MagnetEffect;
        _mode += CheckingIfCanFloat;
    }
    private IEnumerator GoingUp()
    {
        _mode += GoUp;
        yield return new WaitForSeconds(_timeToGoUp);
        _mode -= GoUp;
        _mode += Floating;
    }
    private void GoUp()
    {
        transform.position += Vector3.up * 3 * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer== _mirrorLayer)
        {
            DestroySelf();
        }
    }
    public void DestroySelf()
    {
        GameManager.instance.mirror.TakeFragment();
        Instantiate(_effect,transform.position,transform.rotation);
        Destroy(gameObject);
    }
    public void Floating()
    {
        transform.position += Vector3.up * _floatingAmplitude * Mathf.Cos(Time.time * _floatingSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(1 * _floatingAmplitudeXZ * Mathf.Cos(Time.time * _floatingSpeedXZ),
            transform.rotation.eulerAngles.y,
            1 * _floatingAmplitudeXZ * Mathf.Sin(Time.time * _floatingSpeedXZ)));
    }
    public void Rotating()
    {
        transform.Rotate(transform.up * _rotateSpeed * Time.deltaTime);
    }
}
