using System;
using UnityEngine;

namespace StateMachinePattern
{
    [CreateAssetMenu(fileName = "StateMachinePattern", menuName = "StateMachinePattern/StateTransition", order = 1)]
    public class StateTransition : ScriptableObject
    {
        public State From;
        public State To;
        public StateTransitionCondition Condition;

        public StateTransition(State from, State to, StateTransitionCondition condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }
    }

}