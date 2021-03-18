using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuState : StateMachinePattern.State
{
    public override void OnEnter()
    {
        Time.timeScale = 0f;
        UserInput.Instance.EnableMenuControls();
    }

    public override void OnExit()
    {
        Time.timeScale = 1f;
        UserInput.Instance.EnableGameplayControls();
    }

    public override void Tick()
    {
        
    }
}
