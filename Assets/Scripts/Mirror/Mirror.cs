using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Vector3 _newPosition;
    [SerializeField] private Transform _character;
    [SerializeField] private LayerMask _layersToCheck;

    [SerializeField] private float _upScaleMirrorValue;
    [SerializeField] private float _downScaleMirrorValue;


    [SerializeField] private float _random1X, _random2X;
    public int _cantidad;

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _secondaryTarget;
    [SerializeField] private float _speed=5;
    [SerializeField] private float _distanceToMove;
    [SerializeField] private float _distanceToMoveWhenWaiting;

    private Action _moveDelegate;
    [SerializeField] private float _floatingAmplitude;
    [SerializeField] private float _floatingSpeed;
    [SerializeField] private float _floatingAmplitudeXZ;
    [SerializeField] private float _floatingSpeedXZ;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _material;
    [SerializeField] private Animator _animatorController;
    [SerializeField] private string _animatorControllerChangeTrigger="change";
    [SerializeField] private BaseModel _player;
    [SerializeField] private int _mirrorPieceLayer;
    [SerializeField] private bool _state;
    [SerializeField] private float _timer;
    private Rigidbody _playerRigidBody;
    [SerializeField] private GameObject _jakeMirror;
    [SerializeField] private GameObject _tudorMirror;
    [SerializeField] private GameObject _jakeLifeBar;
    [SerializeField] private GameObject _tudorLifeBar;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pickUpGlassPiece;
    [SerializeField] private AudioClip _breakMirror;
    [SerializeField] private int _layerPlayer;
    private Action _updateDelegate;
    private void Start()
    {
        _material = _renderer.material;
        _animatorController = GetComponent<Animator>();
        _playerRigidBody = _player.gameObject.GetComponent<Rigidbody>();
        GameManager.instance.manaBar.ActualizeBar(0);
    }

    public void GoToFrontOfCharacter()
    {
        RaycastHit hitInfo;
        float distance = _offset.z;
        Vector3 size = new Vector3(2, 2.787702f, 0.5f);
        Vector3 origin = _character.position + Vector3.up;
        if (Physics.BoxCast(origin, size, _character.forward,out hitInfo, _character.rotation, distance, _layersToCheck))
        {
            if (Physics.BoxCast(origin, size, _character.forward, out hitInfo, _character.rotation, 2.5f, _layersToCheck))
            {
                //fedback de que no

            }
            else
            {
                StopAllCoroutines();
                WaitForUse();
                UpScaleMirror();
                transform.rotation = _character.transform.rotation;
                _newPosition.x = _character.position.x + _offset.x;
                _newPosition.y = _character.position.y + _offset.y;
                _newPosition.z = _character.position.z + 2.5f;
                transform.position = _newPosition;
            }
        }
        else
        {
            StopAllCoroutines();
            WaitForUse();
            UpScaleMirror();
            transform.rotation = _character.transform.rotation;
            _newPosition.x = _character.right.x + _offset.x;
            _newPosition.y = _character.position.y + _offset.y;
            _newPosition.z = _character.forward.z + _offset.z;
            transform.position = _character.position + _character.forward * _offset.z + _character.up * _offset.y;
        }
    }
    public void GoToBackOfCharacter()
    {
        _moveDelegate = Move;
        StopCoroutine(CheckDistance());
    }
    public void UpScaleMirror()
    {
        if (!_state)
        {
            _state = !_state;
            _animatorController.SetTrigger(_animatorControllerChangeTrigger);
        }
    }
    public bool GetState()
    {
        return _state;
    }
    public void DownScaleMirror()
    {
        if (_state)
        {
            _state = !_state;
            _animatorController.SetTrigger(_animatorControllerChangeTrigger);
        }
    }
    public Vector3 positionRelativeToMirror(Vector3 offset)
    {
        return transform.position + offset;
    }
    public void BreakMirror()
    {
        _audioSource.clip = _breakMirror;
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.Play();
        for (int i = 0; i < _cantidad; i++)
        {
            var espejos = Resources.Load("Mirror Piece") as GameObject;
            var disparo = Instantiate(espejos, transform.position, Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f));
            StartCoroutine(DisableCollitionWithMirrorFragments(disparo.GetComponent<Collider>()));
            MirrorPiece mp = disparo.GetComponent<MirrorPiece>();
            Rigidbody rb = disparo.GetComponent<Rigidbody>();
            rb.AddForce(disparo.transform.forward * UnityEngine.Random.Range(_random1X, _random2X) 
                + Vector3.up * UnityEngine.Random.Range(1, 3)
                + transform.right * UnityEngine.Random.Range(_random1X, _random2X), ForceMode.Impulse); ;
            rb.AddTorque(disparo.transform.up * UnityEngine.Random.Range(1f, 3f));
        }
        _material.SetFloat("_value", 0);
        GameManager.instance.manaBar.ActualizeBar(0);
    }
    private IEnumerator DisableCollitionWithMirrorFragments(Collider fragment)
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), fragment);
        yield return new WaitForSeconds(0.7f);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), fragment, false);
    }
    private void Update()
    {
        _updateDelegate?.Invoke();
        _moveDelegate?.Invoke();
    }
    public void Floating()
    {
        transform.position += transform.up * _floatingAmplitude * Mathf.Cos(Time.time*_floatingSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(1 * _floatingAmplitudeXZ * Mathf.Cos(Time.time * _floatingSpeedXZ), 
            transform.rotation.eulerAngles.y, 
            1 * _floatingAmplitudeXZ * Mathf.Sin(Time.time * _floatingSpeedXZ)));
    }
    public void Move()
    {
        if(_playerRigidBody.velocity.x!=0|| _playerRigidBody.velocity.z != 0)
        {
            StartCoroutine(Pursuit());
        }
        else
        {
            Arrive();
            transform.position += _velocity * Time.deltaTime;
            timer = 0;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _player.transform.rotation.eulerAngles.y
           , transform.rotation.eulerAngles.z);
     
    }
    void AddForce(Vector3 force)
    {
        _velocity += force;
    }
    private Vector3 _velocity;
    public float maxSpeed;
    public float maxForce;
    public float arriveRadius;
    private float timer;
    private IEnumerator Pursuit()
    {
        timer += Time.deltaTime;
        yield return new WaitForSeconds(0.25f);
        if (_timer >= 1)
        {
            transform.position = _secondaryTarget.position;
        }
        transform.position=(Vector3.Lerp(transform.position, _secondaryTarget.position, timer));

    }
    void Arrive()
    {
        Vector3 desired = _target.position - transform.position;
        desired.Normalize();
        float speed = maxSpeed;
        float distanceToTarget = Vector3.Distance(_target.position, transform.position);
        if (distanceToTarget < arriveRadius)
        {
            speed = maxSpeed * (distanceToTarget / arriveRadius);
        }
        desired *= speed;
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce / 10);
        AddForce(steering);
    }
        public IEnumerator CheckDistance()
    {
        yield return new WaitForSeconds(0.5f);
        if (Vector3.Distance(transform.position, _target.position) > _distanceToMove)
        {
            _moveDelegate = Move;
            StopCoroutine(CheckDistance()); 
        }
        else
        {
            StartCoroutine(CheckDistance());
        }
    }
    public void WaitForUse()
    {
        _moveDelegate = delegate { };
        StartCoroutine(CheckDistanceWhenWaiting());
    }
    public IEnumerator CheckDistanceWhenWaiting()
    {
        yield return new WaitForSeconds(0.5f);
        if (Vector3.Distance(transform.position, _target.position) > _distanceToMoveWhenWaiting)
        {
            DownScaleMirror();
            _moveDelegate = Move;
            StopCoroutine(CheckDistanceWhenWaiting());
        }
        else
        {
            StartCoroutine(CheckDistanceWhenWaiting());
        }
    }
    public void Repair()
    {
        switch (_material.GetFloat("_value"))
        {
            case (0):
                _material.SetFloat("_value", 0.2f);
                GameManager.instance.manaBar.ActualizeBar(0.25f);
                break;
            case (0.2f):
                _material.SetFloat("_value", 0.4f);
                GameManager.instance.manaBar.ActualizeBar(0.5f);
                break;
            case (0.4f):
                _material.SetFloat("_value", 0.6f);
                GameManager.instance.manaBar.ActualizeBar(0.75f);
                break;
            case (0.6f):
                _material.SetFloat("_value", 1.1f);
                GameManager.instance.manaBar.ActualizeBar(1);
                break;
            case (1.1f):
                foreach (var item in GameManager.instance._characters)
                {
                    if (item.Value != GameManager.instance._currentCharacter)
                    {
                        item.Value.GetComponent<PlayerLifeComponent>().OutOfMapHeal(10);
                        break;
                    }
                }
                break;
        }
        _audioSource.clip = _pickUpGlassPiece;
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
    }
 
    public void TakeFragment()
    {
        _player.TakeMirrorFragments();
        Repair();
    }
    public void SetTarget(Transform t, Transform t2)
    {
        _target = t;
        _secondaryTarget = t2;
    }
    public void SetCharacter(Transform t, BaseCharacter.Identity id)
    {
        _character= t;
        SetPlayer();
        switch (id)
        {
            case (BaseCharacter.Identity.jake):
                _jakeMirror.SetActive(true);
                _tudorMirror.SetActive(false);
                break;
            case (BaseCharacter.Identity.Tudor):
                _jakeMirror.SetActive(false);
                _tudorMirror.SetActive(true);
                break;
        }
    }
    public void StartMoving()
    {
        _moveDelegate += Move;
        _updateDelegate += Floating;
        GameManager.instance.manaBar.ActualizeBar(_material.GetFloat("_value"));
    }
    public void SetPlayer()
    {
        _player = _character.gameObject.GetComponent<BaseModel>();
        _playerRigidBody = _player.gameObject.GetComponent<Rigidbody>();
    }
}
