
using UnityEngine;

public class InitializationToGameplayCondition : StateMachinePattern.StateTransitionCondition
{
    [SerializeField]
    private InitializerState InitState;
    public override bool Check => HasInitialized();

    private bool HasInitialized()
    {
        return InitState.Initialized;
    }
}
