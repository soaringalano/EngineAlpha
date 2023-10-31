using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : GameState
{



    public override bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public override bool CanExit()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public override void OnEnter()
    {
        Debug.Log("Game manager is entering Gameplay state......");
        m_stateMachine.EnableGameplayInput();
    }

    public override void OnExit()
    {
        Debug.Log("Game manager is exiting Gameplay state......");
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
