using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerModel : BaseModel
{
    private PlayerMovement _movement;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;

    [SerializeField] private Animator _animator;
    [SerializeField] private RotationofSpeed _rotationOfSPeed;
    [SerializeField] private JumpingFoot _jumpingFoot;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Mirror _mirror;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public ParticleSystem[] dirtParticles;

    public Action<bool> puttingMirror;
    [SerializeField] private float _timeToChangeCharacter;
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
    [SerializeField] private CapsuleCollider _collider;
    public int _tutoLayer = 22;
    public int _winLayer = 23;
    [SerializeField] public AudioClip[] _attackClip;
    [SerializeField] public AudioClip[] _jumpClip;

    [SerializeField] private GameObject[] _audioEffects;
    public Action GetFacon;
    private void Start()
    {
        _movement = GetMovement();
        _character = GetComponent<BaseCharacter>();
        _rotationOfSPeed = GetRotationOfSpeed();
        _movement.moveDirection += _rotationOfSPeed.DeclarationOfDirection;
        _playerAttack = GetPlayerAttak();
        GetFacon += TurnOnWeapon;
        GetJumpingFoot();
        _jumpingFoot.SetJumpForce(_jumpForce);
        if (_cameraTransform == null)
        {
            Debug.Log("camera transform is not asigned");
            gameObject.SetActive(false);
        }
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _jumpingFoot.isGrounded += StopRunning;
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
                                            .SetDashSpeed(_dashSpeed)
                                            .SetDashTime(_dashTime)
                                            .SetModel(this);
        }
        return _movement;
    }
    public AudioSource GetAudioSource()
    {
        return _audioSource;
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
    public JumpingFoot GetJumpingFoot()
    {
        if (_jumpingFoot == null)
        {
            _jumpingFoot=GetComponentInChildren<JumpingFoot>();
            if (_jumpingFoot == null)
            {
                Debug.Log("JumpingFoot not found");
                gameObject.SetActive(false);
            }
        }
        return _jumpingFoot;
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
    public void FindignWeapon(Vector3 lookposition)
    {
        transform.LookAt(lookposition);
        transform.rotation=Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        GetFacon?.Invoke();
    }
    public void StopRunning(bool value)
    {
        if (!value)
        {
            _movement.StopRunning();
        }
    }
    public void TurnOnWeapon()
    {
        StartCoroutine(TurnOnTheWeapon());
    }
    private IEnumerator TurnOnTheWeapon()
    {
        yield return new WaitForSeconds(0.4f);
        _playerAttack.TurnOnWeaponObject();
    }
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
   public override float GetMovementYvalue()
    {
        return _movement.GetYVelocity();
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
    private IEnumerator ChangingCharacter()
    {
        _jumpingFoot.gameObject.SetActive(false);
        changeCharacter?.Invoke(true);
        _rigidBody.useGravity=false;
        _collider.enabled = false;
        _capsuleCollider.enabled=false;
        transform.forward = _mirror.transform.forward;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        _rigidBody.velocity = transform.forward * _jumpIntoTheMirrorSpeed+transform.up* _jumpIntoTheMirrorSpeedY;
        yield return new WaitForSeconds(0.7123753f);
        //cambiar personaje
        _character.ChangeCharacter();
       
    }
    public void GetOutOfTheMirror()
    {
        _jumpingFoot.gameObject.SetActive(false);
        _jumpingFoot.ResetTouchingFloors();
        _rigidBody.useGravity = false;
        _capsuleCollider.enabled = false;
        _collider.enabled = false;

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
        _collider.enabled = true;
        _capsuleCollider.enabled = true;
        _mirror.DownScaleMirror();
        changeCharacter?.Invoke(false);
        _mirror.GoToBackOfCharacter();
        _jumpingFoot.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _winLayer)
        {
            TutorialManager.instance.Win();
        }
    }
    public void PlayAudioSwordSwoshs()
    {
        Instantiate(_audioEffects[UnityEngine.Random.Range(0, _audioEffects.Length)])
            .GetComponent<AudioSource>().pitch= UnityEngine.Random.Range(0.8f,1.5f);
    }
}
