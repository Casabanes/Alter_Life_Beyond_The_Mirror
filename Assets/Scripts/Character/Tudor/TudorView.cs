using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TudorView : MonoBehaviour
{
    private Animator _animator;
    private TudorModel _model;
    private AudioSource _audioSource;
    private string _speedParameter = "speed";
    private string _hasJumpedParameter = "hasJumped";
    private string _isGroundedParameter = "isGrounded";
    private string _attack_Parameter = "attack";
    private string _attack_01Parameter = "attack";
    private string _attack_02Parameter = "attack2";
    private bool _atk=false;
         
    private string _puttingMirror = "putingMirror";
    private string _jumpInTheMirror = "jumpInTheMirror";
    private string _fallingFromMirror = "fallingFromMirror";
    private string _climbing = "climbing";
    private string _reachTheTop = "reachTheTop";

    private Rigidbody _rigidBody;
    private Vector2 _horizontalVelocity;

    private const int _constZero = 0;
    private Action _onUpdateDelegate;
    public TudorView SetAnimator(Animator animator)
    {
        _animator = animator;
        return this;
    }
    public TudorView SetModel(TudorModel playerModel)
    {
        _model = playerModel;
        _atk = false;
        _animator = _model.GetAnimator();
        _rigidBody = _model.GetRigidBody();
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
       
    }
    public void Falling()
    {
            _animator.SetTrigger(_hasJumpedParameter);
    }
    private void Land()
    {
            _animator.SetTrigger(_isGroundedParameter);
    }
    public void Attack_01(bool value)
    {
        if (!value)
        {
            return;
        }
        _animator.SetTrigger(_attack_Parameter);
        _onUpdateDelegate += AttackSound;
        ChangeAttackCall();
    }
    private float timer;
    private void ChangeAttackCall()
    {
        if (!_atk)
        {
            _attack_Parameter=_attack_02Parameter;
            _atk = true;
        }
        else
        {
            _attack_Parameter=_attack_01Parameter;
            _atk = false;
        }
    }
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
    public void Climb(bool value)
    {
        _animator.SetBool(_climbing, value);
        if (value)
        {
            _onUpdateDelegate += ApplyClimbVelocity;
        }
        else
        {
            _onUpdateDelegate -= ApplyClimbVelocity;
            _animator.speed = 1;
        }
    }
    private void ApplyClimbVelocity()
    {
        _animator.speed = _rigidBody.velocity.normalized.magnitude;
    }
    public void ReachTheTop(bool value)
    {
        if (!value)
        {
            return;
        }
        _animator.SetTrigger(_reachTheTop);
    }
}
