using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableState : IState
{
    private DifuntaCorreaAgent _agent;
    private float _time;
    private const int _constZero = 0;
    private FSMDifuntaCorrea _fsm;
    public VulnerableState(DifuntaCorreaAgent agent)
    {
        _agent = agent;
        _fsm = _agent.GetFSM();
    }
    public void OnEnter()
    {
        _agent.animatorCacique.SetBool("Moving", true);
        _time = _constZero;
    }

    public void OnExit()
    {
        _agent.animatorCacique.SetBool("Moving", false);
    }

    public void OnUpdate()
    {
        BeignVulnerable();
    }
    private void BeignVulnerable()
    {
        _time += Time.deltaTime;
        _agent.FloatingEffect();
        if (_time >= _agent._vulnerableTime)
        {
            _fsm.ChangeState(_agent._nextState);
        }
    }
}
