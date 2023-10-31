using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneState : GameState
{



    public override bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.N);
    }

    public override bool CanExit()
    {
        return true;
    }

    public override void OnEnter()
    {
        Debug.Log("Game manager is entering switch scene state......");
        m_stateMachine.EnableGameplayInput();
    }

    public override void OnExit()
    {
        Debug.Log("Game manager is exiting switch scene state......");
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {

    }
}
