using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssaultState : IState
{
    private float _distance;
    private float _distanceToLockAttakPoint;
    private float _distanceToStop = 0.5f;
    private float _time;
    private float _timeToRotate;
    private Transform _target;
    private Vector3 _targetPoint;
    private Vector3 _startingForward;
    private float _assaultSpeed;
    private DifuntaCorreaAgent _agent;
    private FSMDifuntaCorrea _fsm;
    private Action _update;
    private const int _constZero = 0;
    private float _vulnerableTimeAffterThisState = 3;

    public AssaultState(DifuntaCorreaAgent agent)
    {
        _agent = agent;
        _fsm = _agent.GetFSM();
    }
    public void OnEnter()
    {
        _agent.animatorCacique.SetBool("Assaulting", true);
        _target = _agent.GetTarget().transform;
        _assaultSpeed = _agent.GetAssaultSpeed();
        _distanceToLockAttakPoint = _agent.GetAssaultLockAttackDistance();
        _timeToRotate = _agent.GetAssaultRotationTime();
        _update = RotateUntilAssault;
        _startingForward = _agent.transform.forward;
    }

    public void OnExit()
    {
        _agent.animatorCacique.SetBool("Assaulting", false);
        _distance = _constZero;
        _agent.Assault(false);
        _agent.OnOffAssaultParticles(false);
    }

    public void OnUpdate()
    {
        _update();
    }
    public void RotateUntilAssault()
    {
        _agent.FloatingEffect();
        _time += Time.deltaTime;
        _agent.transform.forward = Vector3.Slerp
            (_startingForward, (_target.position - _agent.transform.position).normalized, _time);
        if (_time >= _timeToRotate)
        {
            _update = Assault;
            _agent.Assault(true);
            _agent.OnOffAssaultParticles(true);
            _time = _constZero;
        }
    }
    public void Assault()
    {
        if (_distance > _distanceToLockAttakPoint)
        {
            FinalAssault();
            return;
        }
        Vector3 dir = (_target.position - _agent.transform.position).normalized
            * _assaultSpeed * Time.deltaTime;
        _agent.transform.position += dir;
        _distance += dir.magnitude;
        if (_distance > _distanceToLockAttakPoint)
        {
            _targetPoint = _target.position;
        }
    }
    public void FinalAssault()
    {
        Vector3 direction = (_targetPoint - _agent.transform.position).normalized
            * _assaultSpeed * Time.deltaTime;
        _agent.transform.position += direction;
        if ((_targetPoint - _agent.transform.position).magnitude <= _distanceToStop)
        {
            _agent._nextState = AgentStates.Moving;
            _agent._vulnerableTime = _vulnerableTimeAffterThisState;
            _fsm.ChangeState(AgentStates.Vulnerable);
        }
    }
}
