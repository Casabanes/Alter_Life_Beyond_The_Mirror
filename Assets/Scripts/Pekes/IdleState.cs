using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PekeAgent _agent;
    public IdleState(PekeAgent agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        CheckRange();
    }
    private void CheckRange()
    {
        if (GameManager.instance.GetPlayerIsDead())
        {
            return;
        }
        float distance = Vector3.Distance(GameManager.instance.GetPlayer(), _agent.transform.position);
        if (distance < _agent.range)
        {
             float scream = Random.Range(0, 2);
            if (scream > 0)
            {
                if (!_agent._audioSource.isPlaying)
                {
                    _agent._audioSource.Stop();
                    _agent._audioSource.clip = _agent._pekeInRange;
                    _agent._audioSource.Play();
                    _agent.fsm.ChangeState(AgentStates.Throwing);
                }
            }
        }
    }
}
