using System.Collections;
using System.Collections.Generic;
using CharacterProperties;
using UnityEngine;

public class AddJumpToCharacterBehaviour : StateMachineBehaviour
{
    private CharacterController CharacterController;
    public Vector3 InitialSpeed;
    public float ForwardSpeedMultiplier=1f;

    public Vector3 LastVector;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(CharacterController==null)
        {
            CharacterController=animator.GetComponentInParent<CharacterController>();
        }
        //LastVector=animator.GetComponentInParent<CharacterState>().DesiredVelocity;
        //LastVector.y=0f;
        //OriginalForward=animator.transform.forward.normalized;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Do not change transition offset or fix later
        if(stateInfo.normalizedTime<1f)
        {
            float percentage=Mathf.Clamp((1-stateInfo.normalizedTime),0,1);
            Vector3 JumpForce=InitialSpeed*Time.deltaTime*percentage;
            //JumpForce+= LastVector*ForwardSpeedMultiplier;
            
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
