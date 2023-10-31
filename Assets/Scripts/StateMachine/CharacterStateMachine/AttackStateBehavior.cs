using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateBehavior : StateMachineBehaviour

{
    [SerializeField]
    public ParticleSystem particleSys;

    protected ParticleSystem clone;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clone = Instantiate(particleSys, animator.rootPosition, Quaternion.identity) as ParticleSystem;
        clone.Play();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clone.Stop();
        Destroy(clone);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("On Attack Update ");
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("On Attack Move ");
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("On Attack IK ");
    }
}