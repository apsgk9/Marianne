using System.Collections;
using System.Collections.Generic;
using CharacterInput;
using UnityEngine;
using static AnimatorStateMachineEnums;

public class SetRotationToMovementInputBehaviour : StateMachineBehaviour
{
    private Character Character;

    public SetAt SetAt;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Character==null)
        {
            Character = animator.GetComponentInParent<Character>();
        }
        if(SetAt==SetAt.Enter)
        {
            Vector3 EulerRotation=Character._Locomotion.DesiredCharacterVectorForward.normalized;
            Character._Locomotion.ApplyRotation(Quaternion.Euler(EulerRotation.x,EulerRotation.y,EulerRotation.z)); 
        }                   
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SetAt==SetAt.Exit)
        {
            Vector3 EulerRotation=Character._Locomotion.DesiredCharacterVectorForward.normalized;        
            Character.transform.rotation= Quaternion.Euler(EulerRotation.z,EulerRotation.y,EulerRotation.z); 
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
