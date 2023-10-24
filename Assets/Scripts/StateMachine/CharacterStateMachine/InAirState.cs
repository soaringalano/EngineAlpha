using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: InAirState\n");
        m_stateMachine.ActivateInAirTrigger();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: InAirState\n");
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
        return true;
    }
}
