
using UnityEngine;
using Variables;

public class GamePausingCondition : StateMachinePattern.StateTransitionCondition
{
    public override bool Check => TogglePause();
    [SerializeField]
    //private bool _pauseTriggered;
    private BoolVariable _pauseTriggered;
    //private int id=-1;
    //private static int count =0;

    //it seems _pauseTriggered is false always due to having to conditons being used
    //when built
    private bool TogglePause()
    {
        if(_pauseTriggered.Value)
        {
            _pauseTriggered.SetValue(false);
            return true;
        }
        return false;
    }

    public void PauseMenuKeyPressing()
    {
        Debug.Log("_pauseTriggered.Value: "+_pauseTriggered.Value);
        Debug.Log("PauseMenuKeyPressing");
        _pauseTriggered.SetValue(true);
        Debug.Log("AFTER");
        Debug.Log("_pauseTriggered.Value: "+_pauseTriggered.Value);
        //_pauseTriggered=true;
    }
}
