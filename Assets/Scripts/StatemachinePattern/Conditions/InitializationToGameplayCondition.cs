
using UnityEngine;

public class InitializationToGameplayCondition : StateMachinePattern.StateTransitionCondition
{
    [SerializeField]
    private InitializerState InitState;
    public override bool Condition => HasInitialized();

    private bool HasInitialized()
    {
        //Debug.Log(GameInitializer.hasInitialized);
        //return GameInitializer.hasInitialized;
        return InitState.Initialized;
    }
}
