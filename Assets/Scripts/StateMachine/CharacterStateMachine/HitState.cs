using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : CharacterState
{

    private const float HIT_DURATION = 0.2f;
    private float m_currentStateDuration;

    private AudioSource m_clip;

    public HitState(AudioSource clip)
    {
        m_clip = clip;
    }

    public override void OnEnter()
    {
        if(m_clip != null)
        {
            m_clip.Play();
        }
        m_currentStateDuration = HIT_DURATION;
        m_stateMachine.OnHitStimuliReceived = false;
        m_stateMachine.ActivateIsHitTrigger();
        Debug.Log("Enter state: HitState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
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
        return m_stateMachine.OnHitStimuliReceived;
    }

    public override bool CanExit()
    {
        return m_currentStateDuration < 0;
    }
}
