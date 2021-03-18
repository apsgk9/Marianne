
using UnityEngine;

public class GamePausingCondition : StateMachinePattern.StateTransitionCondition
{
    public override bool Check => TogglePause();
    public bool _pauseTriggered=false;

    private bool TogglePause()
    {
        if(_pauseTriggered)
        {
            _pauseTriggered=false;
            return true;
        }
        return false;
    }

    public void PauseMenuKeyPressing()
    {
        _pauseTriggered=true;
    }
}
