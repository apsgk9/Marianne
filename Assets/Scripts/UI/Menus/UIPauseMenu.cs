using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public GameObject PauseMenu;

    private void Awake()
    {
        PauseMenu.SetActive(false);        
    }
    private void OnEnable()
    {
        UserInput.Instance.PlayerInputActions.PlayerControls.MenuKey.performed += HandleMenuPressed;
        UserInput.Instance.PlayerInputActions.MenuControls.MenuKey.performed += HandleMenuPressed;
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
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.MenuKey.performed -= HandleMenuPressed;
            UserInput.Instance.PlayerInputActions.MenuControls.MenuKey.performed -= HandleMenuPressed;
        } 
        base.OnDestroy();      
    }
    private void HandleMenuPressed(InputAction.CallbackContext obj)
    {
        TogglePause();
    }
    public void TogglePause()
    {
        GameManager.Instance.TogglePauseState();
        bool isPaused = GameManager.Instance.isPaused;
        PauseMenu.SetActive(isPaused);
    }

    private static string GetActionName(string ActionName)
    {
        //PlayerInputActions+MenuControlsActions
        ActionName=ActionName.Replace("Actions","");
        int plusIndex=ActionName.IndexOf('+');
        ActionName=ActionName.Substring(plusIndex+1);
        return ActionName;
    }
}
