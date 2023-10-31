using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        m_possibleStates.Add(new SwitchSceneState());
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

    public void SwitchScene()
    {

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Level1")
        {
            // Show a button to allow scene2 to be switched to.
            Debug.Log("Now loading level 2");

            SceneManager.LoadScene("Level2");
        }
        else if (scene.name == "Level2")
        {
            Debug.Log("Now loading level 3");
            SceneManager.LoadScene("Level3");
        }
        else
        {
            Debug.Log("No more new levels");
        }
    }

}
