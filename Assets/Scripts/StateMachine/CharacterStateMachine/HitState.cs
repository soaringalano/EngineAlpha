using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : CharacterState
{

    public const string KEY_STATUS_TRIGGER_ISHIT = "IsHit";

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
        ActivateIsHitTrigger();
        Debug.Log("Enter state: HitState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
        m_stateMachine.OnHitStimuliReceived = false;
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

    public void ActivateIsHitTrigger()
    {
        m_stateMachine.Animator.SetTrigger(KEY_STATUS_TRIGGER_ISHIT);
    }

}
