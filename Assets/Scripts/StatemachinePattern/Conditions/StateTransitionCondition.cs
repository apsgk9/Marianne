using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachinePattern
{
    public abstract class StateTransitionCondition : ScriptableObject
    {
        public abstract bool Check{get;}
    }
}