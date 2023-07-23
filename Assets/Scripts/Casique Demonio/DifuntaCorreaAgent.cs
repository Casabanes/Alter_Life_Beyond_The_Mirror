using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum AgentStates
    {
        Moving,
        Shooting,
        Assault,
        Rampage,
        BurnAll,
        Vulnerable,
        Idle,
        Throwing
}
public class DifuntaCorreaAgent : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float _speed;
    private FSMDifuntaCorrea _fsm;
    [SerializeField] private AgentStates _startingState;
    [SerializeField] private GameObject _objetiveToKill;
    [SerializeField] private float _timeToRotateBeforeShoot;
    [SerializeField] private float _timeToWaitBeforeShoot;
    [SerializeField] private FireProjectile _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private int _damage;
    private const int _constZero = 0;
    private const int _constOne = 1;
    [SerializeField] private int _shootTimesToDoSomethingElse;
    public Animator animatorCacique;
    
    //Assault things
    [SerializeField] private int _assaultDamage;
    [SerializeField] private float _assaultSpeed;
    [SerializeField] private float _assaultRotationTime;
    [SerializeField] private float _assaultLockAttackDistance;
    [SerializeField] private ParticleSystem[] _assaultParticles;
    [SerializeField] private int _layerPlayer;
    [SerializeField] private float _pushForce;
    [SerializeField] private float _pushUpModifier;
    [SerializeField] private ForceMode _pushMode;
    private Rigidbody _pushTarget;
    private bool _isAssaulting;


    // Rampage Things
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _rampageTime;
    [SerializeField] private float _rampageShootCadency;
    [SerializeField] private Transform[] _rampageShootPoints;
    private int _rampageIndex;

    [SerializeField] private LayerMask _layerFloor;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _fightRange;
    [SerializeField] private Transform _referenceCenterPointOfTheFight;
    public float _vulnerableTime;
    public AgentStates _nextState;
    
    private bool _isFighting;

    #region Geters
    public FSMDifuntaCorrea GetFSM()
    {
        if (_fsm == null)
        {
            Debug.Log("FSM not found");
            gameObject.SetActive(false);
        }
        return _fsm;
    }
    #region Movement
    public float GetSpeed()
    {
        return _speed;
    }
    #endregion
    #region Shoot
    public GameObject GetTarget()
    {
        if (_objetiveToKill == null)
        {
            Debug.Log("Gane yo xd");
            gameObject.SetActive(false);
        }
        return _objetiveToKill;
    }
    public float GetTimeToRotateBeforeShoot()
    {
        if (_timeToRotateBeforeShoot == _constZero)
        {
            Debug.LogWarning("The variable _timeToRotateBeforeShoot value is 0");
        }
        return _timeToRotateBeforeShoot;
    }
    public float GetTimeToWaitBeforeShoot()
    {
        if (_timeToWaitBeforeShoot == _constZero)
        {
            Debug.LogWarning("The variable _timeToWaitBeforeShoot value is 0");
        }
        return _timeToWaitBeforeShoot;
    }
    #endregion
    #region Assault
    public float GetAssaultSpeed()
    {
        return _assaultSpeed;
    }
    public float GetAssaultRotationTime()
    {
        return _assaultRotationTime;
    }
    public float GetAssaultLockAttackDistance()
    {
        return _assaultLockAttackDistance;
    }
    public int GetShootTimesToDoSomethingElse()
    {
        return _shootTimesToDoSomethingElse;
    }
    #endregion
    #region Rampage
    public float GetRampageSpeedRotation()
    {
        return _speedRotation;
    }
    public float GetRampageTime()
    {
        return _rampageTime;
    }
    public float GetRampageShootCadency()
    {
        return _rampageShootCadency;
    }
    #endregion
    #endregion
    void Start()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerCharacter>();
        }
        if (_projectile == null)
        {
            Debug.Log("Projectile is null");
            gameObject.SetActive(false);
        }
        if (_objetiveToKill == null)
        {
            _objetiveToKill = FindObjectOfType<PlayerModel>().gameObject;
        }
        _fsm = new FSMDifuntaCorrea();
        _fsm.AddState(AgentStates.Moving, new MovingState(this));
        _fsm.AddState(AgentStates.Shooting, new ShootingState(this));
        _fsm.AddState(AgentStates.Assault, new AssaultState(this));
        _fsm.AddState(AgentStates.Rampage, new RampageState(this));
        _fsm.AddState(AgentStates.Vulnerable, new VulnerableState(this));

        _fsm.ChangeState(_startingState);
        if (_referenceCenterPointOfTheFight == null)
        {
            _referenceCenterPointOfTheFight = transform;
            Debug.Log("This object is the referencecenterpointofthefight");
        }
    }

    void Update()
    {
        _fsm.Update();
        IsPlayerInRange();
    }
    #region Methods
    public void Shoot()
    {
        if (_objetiveToKill == null)
        {
            Debug.Log("Gané, pupupupu");
            return;
        }
        FireProjectile aux = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
        aux.SetOwner(this);
        aux.transform.forward = (_objetiveToKill.transform.position - aux.transform.position).normalized;
        aux.SetDamage(_damage);
    }
    public void Assault(bool value)
    {
        _isAssaulting = value;
    }
    public void OnOffAssaultParticles(bool value)
    {
        if (value)
        {
            foreach (var particles in _assaultParticles)
            {
                particles.Play();
            }
        }
        else
        {
            foreach (var particles in _assaultParticles)
            {
                particles.Stop();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isAssaulting)
        {
            if (collision.gameObject.layer == _layerPlayer)
            {
                _pushTarget = collision.gameObject.GetComponent<Rigidbody>();
                if (_pushTarget != null)
                {
                    Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                    pushDir.y = Vector3.up.y * _pushUpModifier;
                    _pushTarget.AddForce(pushDir * _pushForce, _pushMode);
                    collision.gameObject.GetComponent<LifeComponent>().TakeDamage(_assaultDamage,collision.transform.position);
                    PlayerLifeComponent player = collision.gameObject.GetComponent<PlayerLifeComponent>();
                    if (player != null)
                    {
                        player.dieWay = PlayerLifeComponent.WaysToDie.Burned;
                    }
                }
            }
        }
    }
    public void RampageShoot()
    {
        if (_objetiveToKill == null)
        {
            Debug.Log("Gané, pupupupu");
            return;
        }
        if (_rampageShootPoints.Length <= _constZero)
        {
            return;
        }
        FireProjectile aux = Instantiate(_projectile, _rampageShootPoints[_rampageIndex].position, _rampageShootPoints[_rampageIndex].rotation);
        aux.SetOwner(this);
        aux.SetDamage(_damage);
        _rampageIndex++;
        _rampageIndex++;
        if (_rampageIndex > _rampageShootPoints.Length)
        {
            _rampageIndex = _constOne;
        }
        if (_rampageIndex == _rampageShootPoints.Length)
        {
            _rampageIndex = _constZero;
        }
    }
    public void FloatingEffect()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, - Vector3.up, out hit, 100f, _layerFloor);
        Vector3 direction = Vector3.up * (0.01f * Mathf.Cos(Time.time * 5));
        if(hit.distance < 0.5f)
        {
            direction.y = hit.point.y + 0.5f;
        }
        transform.position += (Vector3.up *direction.y);
    }

    public bool GetIsFighting()
    {
        return _isFighting;
    }
    public void IsPlayerInRange()
    {
        if (Vector3.Distance(_player.transform.position
            , _referenceCenterPointOfTheFight.position) <= _fightRange)
        {
            _isFighting= true;
        }
        else
        {
            _fsm.ChangeState(AgentStates.Moving);
            _isFighting= false;
        }
    }
    #endregion
}
