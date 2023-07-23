using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PekeAgent : MonoBehaviour
{
    [SerializeField] private AgentStates _startingState;
    public FSMDifuntaCorrea fsm;
    public Animator animator;
    public float damage;
    public float attacksPerSecond;
    [SerializeField] private GameObject axe;
    public float range;
    [SerializeField] private float _upwardMultiplier;
    public AudioClip _pekeInRange;
    public AudioClip _pekeAttacking;
    public AudioClip _pekeOutOfRange;
    public AudioSource _audioSource;
    public float _minPitch = 0.4f;
    public float _maxPitch = 1.6f;
    private Action updateDelegate;
    private void Start()
    {
        float randomPitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
        _audioSource.pitch = randomPitch;
        fsm = new FSMDifuntaCorrea();
        fsm.AddState(AgentStates.Idle, new IdleState(this));
        fsm.AddState(AgentStates.Throwing, new ThrowingState(this));
        fsm.ChangeState(AgentStates.Idle);
        EventManager.instance.pause += Pause;
        updateDelegate += fsm.Update;
    }
    public void Shoot()
    {
        if (GameManager.instance.GetPlayerIsDead())
        {
            return;
        }
        float scream = UnityEngine.Random.Range(0, 2);
        if (scream > 0)
        {
            if (!_audioSource.isPlaying)
            {
            _audioSource.Stop();
            _audioSource.clip = _pekeAttacking;
            _audioSource.Play();
            }
        }
        var auxaxe = Instantiate(axe);
        var lookPos = GameManager.instance.GetPlayer() - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
        var disparo = Instantiate(auxaxe, gameObject.transform.position + (transform.up * 1.4f), Quaternion.identity);
        disparo.transform.forward = transform.forward;
        Rigidbody rb = disparo.GetComponent<Rigidbody>();
        rb.AddForce(GameManager.instance.GetPlayer() - transform.position + transform.up * _upwardMultiplier, ForceMode.Impulse);
        rb.AddTorque(disparo.transform.right * 800f);
    }
    private void Update()
    {
        updateDelegate?.Invoke();
    }
    private void Pause(bool value)
    {
        if (value)
        {
            updateDelegate = delegate { };
        }
        else
        {
            updateDelegate += fsm.Update;
        }
    }

}
