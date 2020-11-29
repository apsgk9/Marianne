using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterProperties;
public class StaminaBehaviour : StateMachineBehaviour
{
    
    //Only one overall stamina must be allowed in the character or else will fail.
    public ICharacterStamina CharacterStamina;
    public int StaminaChange;
    public bool UseInitialCondition=false;
    public string InitialConditionParameterName="Speed";
    public float Condition=3f;
    public int InitialStaminaUsage;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterStamina = animator.GetComponentInParent<CharacterStamina>();
        CharacterStamina.IsStaminaBeingUsed=true;
        if(UseInitialCondition)
        {
            float conditionGiven=animator.GetFloat(InitialConditionParameterName);
            if(Mathf.Approximately(conditionGiven,Condition))
            {
                CharacterStamina.AddStamina(InitialStaminaUsage);
            }
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(StaminaChange!=0)
        {
            float newStamina= StaminaChange*Time.deltaTime;        
            CharacterStamina.AddStamina(newStamina);
        }
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterStamina.IsStaminaBeingUsed=false;
        
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
