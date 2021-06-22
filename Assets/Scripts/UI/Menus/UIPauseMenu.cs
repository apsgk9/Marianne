using System;
using System.Collections;
using System.Collections.Generic;
using StateMachinePattern;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public GameObject PauseMenu;
    public GamePausingCondition GamePausedCondition;

    private void Awake()
    {
        PauseMenu.SetActive(false);        
    }
    private void OnEnable()
    {
        UserInput.Instance.PlayerInputActions.PlayerControls.MenuKey.performed += HandleMenuPressed;
        UserInput.Instance.PlayerInputActions.MenuControls.MenuKey.performed += HandleMenuPressed;

        GameStateMachine.OnGameStateChanged +=StateChanged;
    }
    private void OnDisable()
    {
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.MenuKey.performed -= HandleMenuPressed;
            UserInput.Instance.PlayerInputActions.MenuControls.MenuKey.performed -= HandleMenuPressed;
        }        
    }
    private new void OnDestroy()
    {
        base.OnDestroy();   
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.MenuKey.performed -= HandleMenuPressed;
            UserInput.Instance.PlayerInputActions.MenuControls.MenuKey.performed -= HandleMenuPressed;
        }    
    }
    private void HandleMenuPressed(InputAction.CallbackContext obj)
    {
        TogglePause();
    }
    public void TogglePause()
    {
        
        Debug.Log("444444444");
        GamePausedCondition.PauseMenuKeyPressing();
        Debug.Log("222222222");
    }


    private static string GetActionName(string ActionName)
    {
        //PlayerInputActions+MenuControlsActions
        ActionName=ActionName.Replace("Actions","");
        int plusIndex=ActionName.IndexOf('+');
        ActionName=ActionName.Substring(plusIndex+1);
        return ActionName;
    }


    public void StateChanged(State NewState)
    {
        PauseMenu.SetActive(NewState is PauseMenuState);
    }
}
