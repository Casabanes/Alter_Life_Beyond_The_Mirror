using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingState : IState
{
    private PekeAgent _agent;
    private float _time;
    public ThrowingState(PekeAgent agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
        _time = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        Shooting();
        CheckRange();
    }
    private void Shooting()
    {
        _time += Time.deltaTime;
        if (_time >= _agent.attacksPerSecond)
        {
            if (GameManager.instance.GetPlayerIsDead())
            {
                _agent.fsm.ChangeState(AgentStates.Idle);
            }
            _agent.Shoot();
            _agent.animator.SetTrigger("throwing");
            _time = 0;
        }
    }
    private void CheckRange()
    {
        float distance = Vector3.Distance(GameManager.instance.GetPlayer(), _agent.transform.position);
        if (distance > _agent.range)
        {
            float scream = Random.Range(0, 2);
            if (scream > 0)
            {
                if (!_agent._audioSource.isPlaying)
                {
                    _agent._audioSource.Stop();
                    _agent._audioSource.clip = _agent._pekeOutOfRange;
                    _agent._audioSource.Play();
                    _agent.fsm.ChangeState(AgentStates.Idle);
                }
            }
        }
    }
}
