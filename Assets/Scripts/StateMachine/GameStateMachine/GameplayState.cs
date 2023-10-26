using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : IState
{
    protected Camera m_camera;

    public GamePlayState(Camera camera)
    {
        m_camera = camera;
    }

    public bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public bool CanExit()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public void OnEnter()
    {
        m_camera.enabled = true;
    }

    public void OnExit()
    {
        m_camera.enabled = false;
    }

    public void OnFixedUpdate()
    {
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
    }
}