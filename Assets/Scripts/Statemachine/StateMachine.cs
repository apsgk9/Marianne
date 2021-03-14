using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachinePattern
{
    [CreateAssetMenu(fileName = "StateMachinePattern", menuName = "StateMachinePattern/StateMachine", order = 0)]
    public class StateMachine : ScriptableObject
    {
        [SerializeField]
        private List<StateTransition> _stateTransitions = new List<StateTransition>();
        [SerializeField]
        private List<StateTransition> _anyStateTransitions = new List<StateTransition>();
        private State _currentState;
        public State CurrenState => _currentState;

        public event Action<State> OnStateChanged;

        public void AddTransition(State from, State to, StateTransitionCondition condition)
        {
            var statetransition = new StateTransition(from, to, condition);
            _stateTransitions.Add(statetransition);
        }

        public void AddAnyTransition(State to, StateTransitionCondition condition)
        {
            var stateTransition = new StateTransition(null, to, condition);
            _anyStateTransitions.Add(stateTransition);
        }
        public void SetState(State state)
        {
            if (_currentState == state)
            {
                return;
            }
            _currentState?.OnExit();
            _currentState = state;
            Debug.Log($"Change to state{state}");
            _currentState.OnEnter();

            OnStateChanged?.Invoke(_currentState);
        }

        public void Tick()
        {

            StateTransition transition = CheckForTransition();
            if (transition != null)
            {
                //SetState(transition.To);
            }
            _currentState.Tick();
        }

        private StateTransition CheckForTransition()
        {
            foreach (var transition in _anyStateTransitions)
            {
                if (transition.Condition)
                {
                    return transition;
                }
            }

            foreach (var transition in _stateTransitions)
            {
                if (transition.From == _currentState && transition.Condition)
                {
                    return transition;
                }
            }
            return null;
        }
    }
}
