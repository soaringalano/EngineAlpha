using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Threading;

public class AttackState : CharacterState
{

    public const string KEY_STATUS_TRIGGER_ATTACK = "CommAttack";

    private const float STATE_EXIT_TIMER = 1.0f;

    private float m_currentStateTimer = 0.0f;

    private AudioSource m_clip;

    public AttackState(AudioSource clip)
    {
        m_clip = clip;
        m_efxState = EFXState.EAttack;
    }

    /**
     * if enemies are closing and J is pressed then enter attack state
     */
    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            bool onfloor = m_stateMachine.IsInContactWithFloor();
            //Debug.Log("is on floor?:" + onfloor);
            List<Collider> enemies = m_stateMachine.GetAttackableEnemies();
            bool canenter = onfloor && enemies != null && enemies.Count > 0;
            //Debug.Log("Detected enemy amount:" + (enemies==null?0:enemies.Count) + " , can enter attack status : " + canenter);
            return canenter && Input.GetKeyDown(KeyCode.J);
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }

    public override void OnStart()
    {
        
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: AttackState\n");
        if (m_clip != null)
        {
            m_clip.Play();
        }
        //m_stateMachine.EffectController.PlaySoundFX(GetEFXState(), m_stateMachine.transform.position, 1.0f);
        m_stateMachine.EffectController.PlayParticleFX(GetEFXState(), m_stateMachine.transform.position);
        Time.timeScale = 0.6f;
        m_stateMachine.HitboxController.ActivateHitbox();
        m_stateMachine.CameraShaker.ShakeCameraOnHit();
        ActivateAttackTrigger();
        m_currentStateTimer = STATE_EXIT_TIMER;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting state: AttackState\n");
        //m_stateMachine.DisableAttackAnimation();
        Time.timeScale = 1.0f;
        m_stateMachine.HitboxController.DeactivateHitbox();
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.FixedUpdateQuickDeceleration();
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public void ActivateAttackTrigger()
    {
        m_stateMachine.Animator.SetTrigger(KEY_STATUS_TRIGGER_ATTACK);
    }

}
