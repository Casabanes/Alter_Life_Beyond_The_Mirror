using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class JumpingFoot : MonoBehaviour
{

    [SerializeField] private int _touchingFloors;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    public Action<bool> isGrounded;
    public Action<bool> hasJumped;
    private const int _constZero = 0;
    public bool _isGrounded;
    [SerializeField] private float _sphereCastRadious;
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private int[] _floorLayers;
    private Action _checkFloor= delegate { };
    [SerializeField] private float _minDistanceToDoAnimation;
    private bool _jumpIsInCooldown;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private ColliderChange _colliderChange;

    [SerializeField] private bool _doubleJump;
    public Action<bool> changeCharacter;
    [SerializeField] private float _maxJumpSpeed;
    [SerializeField] private BaseModel _model;

    private void Start()
    {
        if (_rigidBody == null)
        {
            _rigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();
            if (_rigidBody == null)
            {
                Debug.Log("Rigidbody not found");
                gameObject.SetActive(false);
            }
        }
        if (_colliderChange == null)
        {
            _colliderChange= transform.parent.GetComponent<ColliderChange>();
        }
        _checkFloor = CheckGround;
        _checkFloor();
        if(_model == null)
        {
            _model = GetComponent<PlayerModel>();
            if(_model == null)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        _checkFloor?.Invoke();
    }
    public void Jump()
    {
        if (_isGrounded && !_jumpIsInCooldown && _touchingFloors>0)
        {
            hasJumped?.Invoke(true);
            StartCoroutine(DelayJump());
            StartCoroutine(CooldownJump());
        }
        if (!_doubleJump && !_isGrounded)
        {
            _doubleJump = true;
            hasJumped?.Invoke(true);
            StartCoroutine(DelayJump());
            StartCoroutine(CooldownJump());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _floorLayers.Length; i++)
        {
            if(other.gameObject.layer== _floorLayers[i])
            {
                _touchingFloors++;
                _doubleJump = false;
                Grounding();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _floorLayers.Length; i++)
        {
            if (other.gameObject.layer == _floorLayers[i])
            {
                _touchingFloors--;
                if (_touchingFloors == _constZero)
                {
                    _checkFloor = CheckGround;
                    _checkFloor?.Invoke();
                    IsGrounded(false);
                }
            }
        }
    }
    public void IsGrounded(bool value)
    {
        isGrounded?.Invoke(value);
        _isGrounded = value;
    }
    private IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(0.0f);
        if (_colliderChange != null)
        {
            _colliderChange.Change(false);
        }
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _constZero, _rigidBody.velocity.z);
        _rigidBody.AddForce(Vector3.up * _jumpForce, _forceMode);
        if(_rigidBody.velocity.y> _maxJumpSpeed)
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _maxJumpSpeed, _rigidBody.velocity.z);
        }
    }
    private IEnumerator CooldownJump()
    {
        _jumpIsInCooldown = true;
        yield return new WaitForSeconds(_jumpCooldown);
        _jumpIsInCooldown = false;
    }
    public JumpingFoot SetJumpForce(float jumpForce)
    {
        _jumpForce = jumpForce;
        return this;
    }
    public int GetTouchingFloors()
    {
        return _touchingFloors;
    }
    public void CheckGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position+transform.up;
        Vector3 direction = -transform.up;
        float maxDistance = 0.8f;
        if (Physics.SphereCast(origin, _sphereCastRadious
            , direction, out hit, maxDistance, _floorLayer))
        {
            if (hit.distance <= _minDistanceToDoAnimation && _rigidBody.velocity.y <= _constZero)
            {
                Grounding();
                //_checkFloor = delegate { };
            }
        }
        if(_rigidBody.velocity.y>0 && _model.GetMovementYvalue() <= 0)
        {
            IsGrounded(false);
        }
    }
    public void ResetTouchingFloors()
    {
        _touchingFloors = 0;
    }
    public void Grounding()
    {
        IsGrounded(true);
        if (_colliderChange != null)
        {
            _colliderChange.Change(true);
        }
        hasJumped?.Invoke(false);
    }
}
