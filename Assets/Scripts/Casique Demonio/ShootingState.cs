using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : IState
{
    private DifuntaCorreaAgent _agent;
    private Vector3 _targetPosition=Vector3.zero;
    private FSMDifuntaCorrea _fsm;
    private const int _constZero = 0;
    private float _time= _constZero;
    private float _timeToRotate;
    private float _waitingTime = _constZero;
    private float _waitingTimeToShoot=0.5f;
    private int _shootIndex;
    private int _shootTimesToDoSomethingElse;
    private bool _assaultOrRampage;
    private Vector3 _startingForward= Vector3.zero;
    private float _vulnerableTimeAffterThisState=2;
    public ShootingState (DifuntaCorreaAgent agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
        if (_fsm==null)
        {
            _fsm = _agent.GetFSM();
        }
        _shootTimesToDoSomethingElse = _agent.GetShootTimesToDoSomethingElse();
        if (_agent.GetTarget() == null)
        {
            Debug.Log("Critical error: targetPosition not found");
            return;
        }
        else
        {
            _targetPosition = _agent.GetTarget().transform.position;
        }
        _timeToRotate = _agent.GetTimeToRotateBeforeShoot();
        _startingForward = _agent.transform.forward;
        
        if(_shootIndex>= _shootTimesToDoSomethingElse)
        {
            if (_assaultOrRampage)
            {
                _assaultOrRampage = false;
                _shootIndex = _constZero;
                _agent._nextState = AgentStates.Assault;
                _fsm.ChangeState(AgentStates.Vulnerable);
            }
            else
            {
                _assaultOrRampage = true;
                _shootIndex = _constZero;
                _agent._nextState = AgentStates.Rampage;
                _fsm.ChangeState(AgentStates.Vulnerable);
            }
        }

    }
    public void OnExit()
    {
        _agent._vulnerableTime = _vulnerableTimeAffterThisState;
        _shootIndex++;
        _agent.animatorCacique.SetBool("Attacking", false);
    }
    public void OnUpdate()
    {
        RotateUntilShoot();
    }
    public void RotateUntilShoot()
    {
        if (_time >= _timeToRotate)
        {
            WaitingToShoot();
            return;
        }
        Vector3 direction = _targetPosition - _agent.transform.position;
        direction.y = _constZero;
        _agent.transform.forward = Vector3.Slerp(_startingForward, direction, _time / _timeToRotate);
        _time += Time.deltaTime;
        _agent.FloatingEffect();

    }
    public void WaitingToShoot()
    {
        _agent.FloatingEffect();
        _waitingTime += Time.deltaTime;
            _agent.animatorCacique.SetBool("Attacking", true);
        if (_waitingTime >= _waitingTimeToShoot)
        {
            _agent.Shoot();
            _time = _constZero;
            _waitingTime = _constZero;
            _agent._nextState = AgentStates.Moving;
            _fsm.ChangeState(AgentStates.Vulnerable);
        }
    }
}
