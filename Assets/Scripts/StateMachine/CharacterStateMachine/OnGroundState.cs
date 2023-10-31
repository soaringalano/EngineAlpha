using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundState : CharacterState
{

    public const string KEY_STATUS_BOOL_STUN = "Stun";

    public const string KEY_STATUS_TRIGGER_FALLONGROUND = "FallOnGround";


    private const float STUN_DURATION = 1.5f;
    private float m_currentStateDuration;
    private AudioSource m_clip;

    public OnGroundState(AudioSource clip)
    {
        m_clip = clip;
        m_efxState = EFXState.EFall;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering OnGround state");
        if (m_clip != null)
        {
            m_clip.Play();
        }
        //m_stateMachine.EffectController.PlaySoundFX(GetEFXState(), m_stateMachine.transform.position, 1.0f);
        m_stateMachine.EffectController.PlayParticleFX(GetEFXState(), m_stateMachine.transform.position);
        m_stateMachine.EnableTouchGround();
        EnableStun();
        m_currentStateDuration = STUN_DURATION;

    }

    public override void OnExit()
    {
        Debug.Log("Exiting OnGround state");
        m_stateMachine.OnStunStimuliReceived = false;
        DisableStun();
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
        return m_stateMachine.OnStunStimuliReceived ||
               m_stateMachine.IsInContactWithFloor() &&  m_stateMachine.IsFallingFromHigh();
    }

    public override bool CanExit()
    {
        return m_currentStateDuration <= 0;
    }

    public void EnableStun()
    {
        m_stateMachine.Animator.SetBool(KEY_STATUS_BOOL_STUN, true);
        m_stateMachine.OnStunStimuliReceived = true;
    }

    public void DisableStun()
    {
        m_stateMachine.Animator.SetBool(KEY_STATUS_BOOL_STUN, false);
        m_stateMachine.OnStunStimuliReceived = false;
    }

}
