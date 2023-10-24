using System;
using Unity.Mathematics;
using UnityEngine;

public class FreeState : CharacterState
{

    private AudioSource m_clip;

    public FreeState(AudioSource clip)
    {
        m_clip = clip;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        /*var vectorOnFloorF = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        vectorOnFloorF.Normalize();
        
        var vectorOnFloorB = Vector3.ProjectOnPlane(Utils.ROTATE_X_Z_180D(m_stateMachine.Camera.transform.forward), Vector3.up);
        vectorOnFloorB.Normalize();

        var vectorOnFloorL = Vector3.ProjectOnPlane(Utils.ROTATE_X_Z_90D(m_stateMachine.Camera.transform.forward), Vector3.up);
        vectorOnFloorL.Normalize();
        
        var vectorOnFloorR = Vector3.ProjectOnPlane(Utils.ROTATE_X_Z_M90D(m_stateMachine.Camera.transform.forward), Vector3.up);
        vectorOnFloorR.Normalize();
        
        if (Input.GetKey(KeyCode.W))
        {
            m_stateMachine.RB.AddForce(vectorOnFloorF * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_stateMachine.RB.AddForce(vectorOnFloorB * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_stateMachine.RB.AddForce(vectorOnFloorL * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_stateMachine.RB.AddForce(vectorOnFloorR * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
        }

        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxVelocity;
        }

        float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloorF);
        float backwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloorB);
        float leftComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloorL);
        float rightComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloorR);
        m_stateMachine.UpdateAnimatorMovementValues(new Vector2(leftComponent-rightComponent, forwardComponent-backwardComponent));*/

        FixedUpdateRotateWithCamera();

        if (m_stateMachine.CurrentDirectionalInputs == Vector2.zero)
        {
            m_stateMachine.FixedUpdateQuickDeceleration();
            return;
        }
        Vector2 velocity = m_stateMachine.CurrentDirectionalInputs;
        float speed = velocity.magnitude;
        ApplyMovementsOnFloorFU(velocity);
        if (m_clip != null)
        {
            if(speed > 0)
            {
                m_clip.Play();
            }
            else
            {
                m_clip.Stop();
            }
        }
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        var currentMaxSpeed = m_stateMachine.MaxVelocity;

        if (m_stateMachine.RB.velocity.magnitude > currentMaxSpeed)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= currentMaxSpeed;
        }
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }

    private void FixedUpdateRotateWithCamera()
    {
        var forwardCamOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        m_stateMachine.RB.transform.LookAt(forwardCamOnFloor + m_stateMachine.RB.transform.position);
    }

}
