using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : CharacterState
{

    public const string KEY_STATUS_TRIGGER_ISHIT = "IsHit";

    private const float HIT_DURATION = 0.5f;
    private float m_currentStateDuration;

    private AudioSource m_clip;

    public HitState(AudioSource clip)
    {
        m_clip = clip;
        m_efxState = EFXState.EHit;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering Hit state");
        if (m_clip != null)
        {
            m_clip.Play();
        }
        //m_stateMachine.EffectController.PlaySoundFX(GetEFXState(), m_stateMachine.transform.position, 1.0f);
        m_stateMachine.EffectController.PlayParticleFX(GetEFXState(), m_stateMachine.transform.position);
        m_currentStateDuration = HIT_DURATION;
        m_stateMachine.HitboxController.ActivateHitbox();
        m_stateMachine.CameraShaker.ShakeCameraOnHit();
        ActivateIsHitTrigger();
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Hit State");
        m_stateMachine.HitboxController.DeactivateHitbox();
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
