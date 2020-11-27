using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedToCharacterBehaviour : StateMachineBehaviour
{
    private CharacterController CharacterController;
    public Vector3 InitialSpeed;
    private float LastSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(CharacterController==null)
        {
            CharacterController=animator.GetComponentInParent<CharacterController>();
        }
        LastSpeed=animator.GetFloat("Speed");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime<1f)
        {
            float percentage=(1-stateInfo.normalizedTime);
            Vector3 JumpForce=InitialSpeed*Time.deltaTime*percentage;
            JumpForce+= animator.transform.forward*LastSpeed*Time.deltaTime*2;
            
            CharacterController.Move(JumpForce);
        }
        
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
