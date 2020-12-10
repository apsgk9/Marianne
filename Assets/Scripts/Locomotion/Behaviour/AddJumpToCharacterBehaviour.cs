using System.Collections;
using System.Collections.Generic;
using CharacterProperties;
using UnityEngine;

public class AddJumpToCharacterBehaviour : StateMachineBehaviour
{
    private ICharacterMover CharacterMover;
    public Vector3 InitialSpeed;
    public float ForwardSpeedMultiplier=1f;

    public Vector3 LastVector;
    public string SpeedParameterName="Speed";
    public float Speed;
    public float Height;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(CharacterMover==null)
        {
            CharacterMover=animator.GetComponentInParent<ICharacterMover>();
        }
        Speed=animator.GetFloat(SpeedParameterName);

        LastVector=animator.transform.forward.normalized;        
        Vector3 JumpForce=InitialSpeed;
        JumpForce+= LastVector*ForwardSpeedMultiplier*Speed;
        CharacterMover.AddVelocity(JumpForce);
        CharacterMover.Jump(Height);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //Do not change transition offset or fix later
    //    if(stateInfo.normalizedTime<1f)
    //    {
    //        float percentage=Mathf.Clamp((1-stateInfo.normalizedTime),0,1);
    //        Vector3 JumpForce=InitialSpeed*Time.deltaTime*percentage;
    //        JumpForce+= LastVector*Time.deltaTime*ForwardSpeedMultiplier*Speed;
    //        
    //        CharacterMover.AddExtraMotion(JumpForce);
    //    }        
    //    
    //}

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
