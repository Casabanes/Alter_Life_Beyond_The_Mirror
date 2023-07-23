using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampageState : IState
{
    private DifuntaCorreaAgent _agent;
    private FSMDifuntaCorrea _fsm;
    private float _rotationSpeed;
    private float _time;
    private float _rotationTime;
    private float _shootTime;
    private float _shootCadency;
    private const int _constZero = 0;
    private float _vulnerableTimeAffterThisState = 3;

    public RampageState(DifuntaCorreaAgent agent)
    {
        _agent = agent;
        _fsm = _agent.GetFSM();
    }
    public void OnEnter()
    {
        _agent.animatorCacique.SetBool("Rampaging", true);

        _rotationTime = _agent.GetRampageTime();
        _rotationSpeed = _agent.GetRampageSpeedRotation();
        _shootCadency = _agent.GetRampageShootCadency();
    }
    public void OnExit()
    {
        _agent.animatorCacique.SetBool("Rampaging", false);
    }
    public void OnUpdate()
    {
        RotateWhileIsInThisState();
        ShootToo();
    }
    public void RotateWhileIsInThisState()
    {
        _time += Time.deltaTime;
        _agent.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        if (_time > _rotationTime)
        {
            _time = _constZero;
            _shootTime = _constZero;
            _agent._nextState = AgentStates.Moving;
            _agent._vulnerableTime = _vulnerableTimeAffterThisState;
            _fsm.ChangeState(AgentStates.Vulnerable);
        }
    }
    public void ShootToo()
    {
        _shootTime += Time.deltaTime;
        if (_shootTime >= _shootCadency)
        {
            _shootTime = _constZero;
            _agent.RampageShoot();
        }
    }
}
