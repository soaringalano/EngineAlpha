using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterState
{

    public const string KEY_STATUS_BOOL_INAIR = "InAir";

    public override void OnEnter()
    {
        Debug.Log("Enter state: InAirState\n");
        EnableInAir();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: InAirState\n");
        DisableInAir();
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.InAirFixedUpdateFallHeight();
        ApplyMovementsOnFloorFU(m_stateMachine.CurrentDirectionalInputs);
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        //TODO MF: Explications nécessaires de ce code pour les élèves
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.InAirAccelerationValue, ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(IState currentState)
    {
        return !m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();
    }

    public void EnableInAir()
    {
        m_stateMachine.Animator.SetBool(KEY_STATUS_BOOL_INAIR, true);
    }

    public void DisableInAir()
    {
        m_stateMachine.Animator.SetBool(KEY_STATUS_BOOL_INAIR, false);
    }

}
