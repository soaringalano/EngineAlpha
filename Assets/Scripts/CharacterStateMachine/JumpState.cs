using UnityEngine;

public class JumpState : CharacterState
{
    //private const float STATE_EXIT_TIMER = 1f;
    //private float m_currentStateTimer = 0.0f;

    public override void OnEnter()
    {
        //Debug.Log("Enter state: JumpState\n");

        m_stateMachine.DisableTouchGround();
        m_stateMachine.ActivateJumpTrigger();
        //Effectuer le saut
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        //m_currentStateTimer = STATE_EXIT_TIMER;
    }

    public override void OnExit()
    {
        //Debug.Log("Exit state: JumpState\n");
        //m_stateMachine.DeactivateJumpTrigger();
        m_stateMachine.EnableTouchGround();
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        //m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return m_stateMachine.IsInContactWithFloor() && Input.GetKeyDown(KeyCode.Space);
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();//m_currentStateTimer <= 0;
    }
}
