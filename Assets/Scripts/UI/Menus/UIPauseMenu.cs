using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public GameObject PauseMenu;
    private PlayerInputActions _inputActions;

    public InputAction HandleMenuPress { get; private set; }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        PauseMenu.SetActive(false);        
        _inputActions.MenuControls.Disable();
        _inputActions.PlayerControls.Enable();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.PlayerControls.MenuKey.performed += HandleMenuPressed;
        _inputActions.MenuControls.MenuKey.performed += HandleMenuPressed;
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.PlayerControls.MenuKey.performed -= HandleMenuPressed;
        _inputActions.MenuControls.MenuKey.performed -= HandleMenuPressed;
    }
    private void HandleMenuPressed(InputAction.CallbackContext obj)
    {
        TogglePause();
    }
    public void TogglePause()
    {
        Debug.Log("PAUSING");
        GameManager.Instance.TogglePauseState();
        bool isPaused = GameManager.Instance.isPaused;
        PauseMenu.SetActive(isPaused);
        if(isPaused)
        {            
            _inputActions.MenuControls.Enable();
            _inputActions.PlayerControls.Disable();
        }
        else
        {
            _inputActions.MenuControls.Disable();
            _inputActions.PlayerControls.Enable();
        }
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
