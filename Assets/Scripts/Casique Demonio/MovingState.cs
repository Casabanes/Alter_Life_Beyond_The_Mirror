using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState
{
    private const int _constZero = 0;
    private float _minDistanceToChangeIndex=0.3f;
    private DifuntaCorreaAgent _agent;
    private FSMDifuntaCorrea _fsm;
    private int _waypointIndex = _constZero;
    private float _speed;
    private Vector3 _startingForward = Vector3.zero;
    private float _time = _constZero;
    private float _timeToRotate=0.5f;
    public MovingState(DifuntaCorreaAgent agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
        _agent.animatorCacique.SetBool("Moving", true);

        if (_fsm == null)
        {
            _fsm=_agent.GetFSM();
        }
        if (_speed == _constZero)
        {
            _speed = _agent.GetSpeed();
        }
        _startingForward = _agent.transform.forward;
    }

public void OnExit()
    {
        _time = _constZero;
        _agent.animatorCacique.SetBool("Moving",false);
    }

    public void OnUpdate()
    {
        Move();
    }
    public void Move()
    {
        Vector3 dir = (_agent.waypoints[_waypointIndex].position - _agent.transform.position);

        if (dir.magnitude < _minDistanceToChangeIndex)
        {
            _waypointIndex++;
            if (_waypointIndex >= _agent.waypoints.Count)
            {
                _waypointIndex = _constZero;
            }
            if (_agent.GetIsFighting())
            {
                _fsm.ChangeState(AgentStates.Shooting);
            }
        }
        _agent.transform.position += dir.normalized * _speed * Time.deltaTime;
        dir.y = _constZero;
        RotateToDirection(dir.normalized);
        _agent.FloatingEffect();
    }
    public void RotateToDirection(Vector3 direction)
    {
        _agent.FloatingEffect();
        _time += Time.deltaTime;
        if (_time >= _timeToRotate)
        {
            return;
        }
        _agent.transform.forward = Vector3.Slerp(_startingForward, direction, _time/_timeToRotate);
    }
}
