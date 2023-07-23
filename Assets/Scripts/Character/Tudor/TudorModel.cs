using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TudorModel : BaseModel
{
    private PlayerMovement _movement;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private RotationofSpeed _rotationOfSPeed;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Mirror _mirror;
    [SerializeField] private AudioSource _audioSource;
    public Action<bool> puttingMirror;
    [SerializeField] private float _jumpIntoTheMirrorSpeed;
    [SerializeField] private float _jumpIntoTheMirrorSpeedY;
    [SerializeField] private float _jumpOutOfTheMirrorSpeed;
    [SerializeField] private float _changingCharacterYOffset;
    [SerializeField] private float _changingCharacterZOffset;
    [SerializeField] private Vector3 _changeCharacterSpawnPosition;
    public Action<bool> changeCharacter;
    public Action<bool> changeCharacterOut;
    [SerializeField] private BaseCharacter _character;
    [SerializeField] private LayerMask _layerMirror;
    [SerializeField] private bool _canClimb;
    [SerializeField] private bool _isClimbing;
    [SerializeField] private int _climbableLayer;
    [SerializeField] private int _climbabtopLayer;
    [SerializeField] private JumpingFoot _jumpingFoot;

    public Action<bool> climbing;
    public Action<bool> climbtop;
    public int _tutoLayer = 22;
    public int _winLayer = 23;
    [SerializeField] public AudioClip[] _attackClip;

    private void Start()
    {
        _movement = GetMovement();
        _character = GetComponent<BaseCharacter>();
        _rotationOfSPeed = GetRotationOfSpeed();
        _movement.moveDirection += _rotationOfSPeed.DeclarationOfDirection;
        _playerAttack = GetPlayerAttak();
        if (_cameraTransform == null)
        {
            Debug.Log("camera transform is not asigned");
            gameObject.SetActive(false);
        }
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _jumpingFoot.SetJumpForce(0);
        _movement.SetJumpingFoot(_jumpingFoot);
    }

    private void Update()
    {
    }

    #region GetersOfComponents
    public PlayerMovement GetMovement()
    {
        if (_movement == null)
        {
            _movement = new PlayerMovement().SetTransform(transform)
                                            .SetRigidBody(_rigidBody)
                                            .SetSpeed(_speed)
                                            .SetCameraTransform(_cameraTransform)
                                            .SetModel(this);
        }
        return _movement;
    }
    public Animator GetAnimator()
    {
        if (_animator == null)
        {
            CheckVariableComponents();
        }
        return _animator;
    }
    public Rigidbody GetRigidBody()
    {
        if (_rigidBody == null)
        {
            CheckVariableComponents();
        }
        return _rigidBody;
    }
    public JumpingFoot GetJumpingFoot()
    {
        if (_jumpingFoot == null)
        {
            _jumpingFoot = GetComponentInChildren<JumpingFoot>();
            if (_jumpingFoot == null)
            {
                Debug.Log("JumpingFoot not found");
                gameObject.SetActive(false);
            }
        }
        return _jumpingFoot;
    }
    public RotationofSpeed GetRotationOfSpeed()
    {
        if (_rotationOfSPeed == null)
        {
            _rotationOfSPeed = GetComponent<RotationofSpeed>();
            if (_rotationOfSPeed == null)
            {
                _rotationOfSPeed = gameObject.AddComponent<RotationofSpeed>();
            }
        }
        return _rotationOfSPeed;
    }
    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
    public PlayerAttack GetPlayerAttak()
    {
        if (_playerAttack == null)
        {
            _playerAttack = GetComponent<PlayerAttack>();
            if (_playerAttack == null)
            {
                Debug.Log("PlayerAttack not found");
                gameObject.SetActive(false);
            }
        }
        return _playerAttack;
    }
    #endregion
    public void CheckVariableComponents()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody>();
            if (_rigidBody == null)
            {
                Debug.Log(_rigidBody.GetType() + " component not found");
                gameObject.SetActive(false);
            }
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.Log(_animator.GetType() + " component not found");
                gameObject.SetActive(false);
            }
        }
    }
    public void PutTheMirror()
    {
        if (Physics.Raycast(transform.position + transform.up * 1.5f, transform.forward * 2, _layerMirror)
              && _currentMirrorPieces >= _maxMirrorPieces && _mirror.GetState())
        {
            ChangeCharacter(true);
        }
        else
        {
            StartCoroutine(PuttingTheMirror());
        }
    }
    public override float GetMovementYvalue()
    {
        return _movement.GetYVelocity();
    }
    private IEnumerator PuttingTheMirror()
    {
        puttingMirror(true);
        _rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.25f);
        _mirror.GoToFrontOfCharacter();
        yield return new WaitForSeconds(0.5f);
        puttingMirror(false);
    }
    public void ChangeCharacter(bool value)
    {
        if (value)
        {
            StartCoroutine(ChangingCharacter());
        }
        //0.7123753 tiempo que tarda en entrar al espejo
    }
    public void Climb()
    {
        if (!_canClimb)
        {
            return;
        }
        if (_isClimbing)
        {
            ClimbThings(false);
        }
        else
        {
            ClimbThings(true);
        }
       
    }
    private void ClimbThings(bool value)
    {
        _rotationOfSPeed.enabled = !value;
        _rigidBody.useGravity = !value;
        _rigidBody.velocity = Vector3.zero;

        transform.rotation = climbRotation;
        _movement.UseXYMovment(value);
        _isClimbing = !_isClimbing;
        climbing?.Invoke(_isClimbing);
    }
    private IEnumerator ChangingCharacter()
    {
        _jumpingFoot.gameObject.SetActive(false);
        changeCharacter?.Invoke(true);
        _rigidBody.useGravity = false;
        _capsuleCollider.enabled = false;
        transform.forward = _mirror.transform.forward;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        _rigidBody.velocity = transform.forward * _jumpIntoTheMirrorSpeed + transform.up * _jumpIntoTheMirrorSpeedY;
        yield return new WaitForSeconds(0.7123753f);
        //cambiar personaje
        _character.ChangeCharacter();

    }
    public void GetOutOfTheMirror()
    {
        _jumpingFoot.gameObject.SetActive(false);
        _rigidBody.useGravity = false;
        _capsuleCollider.enabled = false;
        changeCharacterOut?.Invoke(true);
        transform.forward = _mirror.transform.forward;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));


        _changeCharacterSpawnPosition = _mirror.positionRelativeToMirror(Vector3.up * _changingCharacterYOffset + Vector3.forward * _changingCharacterZOffset);
        transform.position = _changeCharacterSpawnPosition;
        transform.forward = -transform.forward;
        _currentMirrorPieces = 0;
        _rigidBody.velocity = transform.forward * _jumpOutOfTheMirrorSpeed;
        StartCoroutine(RecoveryAfterChange());
    }
    private IEnumerator RecoveryAfterChange()
    {
        yield return new WaitForSeconds(0.6f);
        _rigidBody.useGravity = true;
        _capsuleCollider.enabled = true;
        _mirror.DownScaleMirror();
        changeCharacter?.Invoke(false);
        _mirror.GoToBackOfCharacter();
        _jumpingFoot.gameObject.SetActive(true);
        _jumpingFoot.ResetTouchingFloors();
    }
    private Quaternion climbRotation;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == _climbableLayer)
        {
            _canClimb = true;
            climbRotation = collision.gameObject.transform.rotation;
        }
        if(collision.gameObject.layer== _climbabtopLayer && _isClimbing)
        {
                _rigidBody.useGravity = true;
                _rotationOfSPeed.enabled = true;
                _movement.UseXYMovment(false);
                _canClimb = false;
                _isClimbing = false;
                climbing?.Invoke(_isClimbing);
                climbtop?.Invoke(true);
                _rigidBody.AddForce((transform.forward + transform.up) * 300,ForceMode.Impulse);
            if (_rigidBody.velocity.y > 5)
            {
                _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 5, _rigidBody.velocity.z);
            }
        }
        if (collision.gameObject.layer == _winLayer)
        {
            TutorialManager.instance.Win();
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == _climbableLayer)
        {
            _canClimb = true;
            climbRotation = collision.gameObject.transform.rotation;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == _climbableLayer)
        {
            _rigidBody.useGravity = true;
            _rotationOfSPeed.enabled = true;
            _movement.UseXYMovment(false);
            _canClimb = false;
            _isClimbing = false;
            climbing?.Invoke(_isClimbing);
            climbRotation = Quaternion.identity;
        }
    }
}
