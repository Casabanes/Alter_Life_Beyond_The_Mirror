using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerView 
{
    private Animator _animator;
    private PlayerModel _model;
    private JumpingFoot _jumpingFoot;
    private AudioSource _audioSource;
    private string _speedParameter= "speed";
    private string _isGroundedParameter = "isGrounded";
    private string _hasJumpedParameter = "hasJumped";
    private string _attack_01Parameter = "attack";
    private string _puttingMirror = "putingMirror";
    private string _jumpInTheMirror = "jumpInTheMirror";
    private string _fallingFromMirror = "fallingFromMirror";
    private string _pickUp = "pickUp";
    private string _ySpeedParameter = "ySpeed";
    private Rigidbody _rigidBody;
    private Vector2 _horizontalVelocity;
    private const int _constZero = 0;
    private Action _onUpdateDelegate;
    public PlayerView SetAnimator(Animator animator)
    {
        _animator = animator;
        return this;
    }
    public PlayerView SetModel(PlayerModel playerModel)
    {
        _model = playerModel;
        _animator = _model.GetAnimator();
        _rigidBody = _model.GetRigidBody();
        _jumpingFoot = _model.GetJumpingFoot();
        _audioSource = _model.GetAudioSource();
        return this;
    }
    public void OnUpdate()
    {
        Move();
        _onUpdateDelegate?.Invoke();
    }
    public void Move()
    {
        _horizontalVelocity.x = _rigidBody.velocity.x;
        _horizontalVelocity.y = _rigidBody.velocity.z;
        _animator.SetFloat(_speedParameter, _horizontalVelocity.magnitude);
        if (_horizontalVelocity.magnitude != 0 && _animator.GetBool(_isGroundedParameter))
        {
            foreach (var item in _model.dirtParticles)
            {
                if (!item.isPlaying)
                {
                    item.Play();
                }
            }
        }
        else
        {
            foreach (var item in _model.dirtParticles)
            {
                item.Stop();
            }
        }
    }
    public void JumpingState(bool value)
    {
        _animator.SetBool(_isGroundedParameter, value);
    }
    public void Jumped(bool value)
    {
        if (value)
        {
            _animator.SetTrigger(_hasJumpedParameter);
        }
        if (value)
        {
            _onUpdateDelegate += JumpSound;
        }
    }
    public void Attack_01(bool value)
    {
        if (!value)
        {
            return;
        }
        _onUpdateDelegate += AttackSound;
        _animator.SetTrigger(_attack_01Parameter);
    }
    private float timer;

    public void AttackSound()
    {
        timer += Time.deltaTime;
        if (timer >= 0.2f)
        {
            timer = 0;
            _onUpdateDelegate -= AttackSound;
            _audioSource.Stop();
            _audioSource.clip = _model._attackClip[UnityEngine.Random.Range(0, _model._attackClip.Length)];
            _audioSource.Play();
            _model.PlayAudioSwordSwoshs();
        }
    }
    private float timer2;

    public void JumpSound()
    {
            _onUpdateDelegate -= JumpSound;
            _audioSource.Stop();
            _audioSource.clip = _model._jumpClip[UnityEngine.Random.Range(0, _model._jumpClip.Length)];
            _audioSource.Play();
    }
    public void PutTheMirror(bool value)
    {
        if (!value)
        {
            return;
        }
        _animator.SetTrigger(_puttingMirror);
    }
    public void JumpInTheMirror(bool value)
    {
        if (!value)
        {
            return;
        }
        _animator.SetTrigger(_jumpInTheMirror);
    }
    public void JumpFromTheMirror(bool value)
    {
        if (!value)
        {
            return;
        }
        JumpingState(true);
        _animator.SetTrigger(_fallingFromMirror);
    }
    public void PickUp()
    {
        _animator.enabled = false;
        _animator.enabled = true;
        _animator.SetTrigger(_pickUp);
    }
}
