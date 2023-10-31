using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : AbstractStateMachine<GameState>
{

    [SerializeField]
    public CharacterControllerStateMachine CharacterController;

    [SerializeField]
    public CameraController CameraController;

    [SerializeField]
    public VirtualCameraController VirtualCameraController;

    [SerializeField]
    public TimelineController TimelineController;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        foreach (var state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameState>();
        m_possibleStates.Add(new GamePlayState());
        m_possibleStates.Add(new CinematicState());
    }

    public void EnableGameplayInput()
    {
        if(CharacterController != null)
        {
            CharacterController.AcceptInput = true;
        }
        if(CameraController != null)
        {
            CameraController.AcceptInput = true;
        }
        if(VirtualCameraController != null)
        {
            VirtualCameraController.AcceptInput = true;
        }
        if (CameraController != null)
        {
            CameraController.AcceptInput = true;
        }
    }

    public void DisableGameplayInput()
    {
        if (CharacterController != null)
        {
            CharacterController.AcceptInput = false;
        }
        if (CameraController != null)
        {
            CameraController.AcceptInput = false;
        }
        if (VirtualCameraController != null)
        {
            VirtualCameraController.AcceptInput = false;
        }
        if (CameraController != null)
        {
            CameraController.AcceptInput = false;
        }
    }

}
