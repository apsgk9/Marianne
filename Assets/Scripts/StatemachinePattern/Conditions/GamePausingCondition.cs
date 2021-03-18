
public class GamePausingCondition : StateMachinePattern.StateTransitionCondition
{
    public override bool Condition => TogglePause();
    private bool _pauseTriggered;

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
