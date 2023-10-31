using UnityEngine;

public class JumpState : CharacterState
{

    public const string KEY_STATUS_TRIGGER_JUMP = "Jump";

    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;

    private AudioSource m_clip;

    public JumpState(AudioSource clip)
    {
        m_clip = clip;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");

        if (m_clip != null)
        {
            m_clip.Play();
        }
        m_stateMachine.DisableTouchGround();
        ActivateJumpTrigger();
        //Effectuer le saut
        m_stateMachine.Jump();
        m_currentStateTimer = STATE_EXIT_TIMER;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: JumpState\n");
        //m_stateMachine.DeactivateJumpTrigger();
        //m_stateMachine.EnableTouchGround();
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        //This must be run in Update absolutely
        return currentState is FreeState &&
                Input.GetKeyDown(KeyCode.Space);
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }

    public void ActivateJumpTrigger()
    {
        m_stateMachine.Animator.SetTrigger(KEY_STATUS_TRIGGER_JUMP);
    }

}
