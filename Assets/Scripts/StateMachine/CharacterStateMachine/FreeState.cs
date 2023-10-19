using System;
using Unity.Mathematics;
using UnityEngine;

public class FreeState : CharacterState
{
    public override void OnEnter()
    {
        //Debug.Log("Enter state: FreeState\n");
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        var vectorOnFloorF = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
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
        m_stateMachine.UpdateAnimatorMovementValues(new Vector2(leftComponent-rightComponent, forwardComponent-backwardComponent));

        //TODO 31 AOÛT:
        //Appliquer les déplacements relatifs à la caméra dans les 3 autres directions
        //Avoir des vitesses de déplacements maximales différentes vers les côtés et vers l'arrière
        //Lorsqu'aucun input est mis, décélérer le personnage rapidement

        //Debug.Log(m_stateMachine.RB.velocity.magnitude);


    }

    public override void OnExit()
    {
        //Debug.Log("Exit state: FreeState\n");
    }

    public override bool CanEnter()
    {
        //Je ne peux entrer dans le FreeState que si je touche le sol
        return m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }
}
