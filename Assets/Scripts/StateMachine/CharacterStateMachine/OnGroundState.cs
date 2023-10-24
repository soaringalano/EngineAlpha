using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundState : CharacterState
{
    private const float STUN_DURATION = 1.5f;
    private float m_currentStateDuration;
    private AudioSource m_clip;

    public OnGroundState(AudioSource clip)
    {
        m_clip = clip;
    }

    public override void OnEnter()
    {
        if (m_clip != null)
        {
            m_clip.Play();
        }
        m_stateMachine.OnStunStimuliReceived = false;
        m_stateMachine.EnableStun();
        m_currentStateDuration = STUN_DURATION;

        Debug.Log("Enter state: GroundState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: GroundState\n");
        m_stateMachine.DisableStun();
        m_stateMachine.EnableTouchGround();
        m_stateMachine.InAirResetFallHeight();
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.FixedUpdateQuickDeceleration();
    }

    public override void OnUpdate()
    {
        m_currentStateDuration -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.OnStunStimuliReceived || m_stateMachine.IsFallingFromHigh();
    }

    public override bool CanExit()
    {
        return m_currentStateDuration <= 0;
    }
}
