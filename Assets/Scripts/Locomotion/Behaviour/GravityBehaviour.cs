using System.Collections;
using System.Collections.Generic;
using CharacterProperties;
using UnityEngine;
using static AnimatorStateMachineEnums;

public class GravityBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public bool SetAtEnter=false;
    public bool SetAtExit=false;
    IMover CharacterMover;
    ICharacterState CharacterState;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(CharacterMover==null)
        {
            //get current velocity
            CharacterMover=animator.GetComponentInParent<IMover>();
        }
        if(CharacterState==null)
        {
            //get current velocity
            CharacterState=animator.GetComponentInParent<CharacterState>();
        }

        if(SetAtEnter)
        {
            //get current velocity
            CharacterMover.SetVelocity(CharacterState.ActualCurrentVelocity+Physics.gravity*Time.deltaTime);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(SetAtExit)
        {
            //get current velocity
            CharacterMover.SetVelocity(CharacterState.ActualCurrentVelocity+Physics.gravity*Time.deltaTime);
        }   
    }

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
