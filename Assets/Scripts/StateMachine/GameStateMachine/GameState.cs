using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : IState
{

    protected GameManagerSM m_stateMachine;

    public void OnStart(GameManagerSM stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public virtual bool CanEnter(IState currentState)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnUpdate()
    {
    }
}
