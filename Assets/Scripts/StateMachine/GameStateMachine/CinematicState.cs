using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicState : GameState
{


    public override bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public override bool CanExit()
    {
        return !m_stateMachine.TimelineController.IsPlaying() || Input.GetKeyDown(KeyCode.G);
    }

    public override void OnEnter()
    {
        Debug.Log("Game manager is entering Cinematic state......");
        m_stateMachine.DisableGameplayInput();
        m_stateMachine.TimelineController.StartTimeline();
    }

    public override void OnExit()
    {
        Debug.Log("Game manager is exiting Cinematic state......");
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
