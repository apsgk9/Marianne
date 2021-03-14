using System.Collections;
using System.Collections.Generic;
using StateMachinePattern;
using UnityEngine;

public class DefaultCondition : StateTransitionCondition
{
    public override bool Condition => ConditionCheck();

    public bool ConditionCheck()
    {
        return true;        
    }

}
