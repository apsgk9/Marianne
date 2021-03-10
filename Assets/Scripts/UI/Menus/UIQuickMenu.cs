using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIQuickMenu : Singleton<UIQuickMenu>
{
    public GameObject QuickMenu;
    private void Awake()
    {
        QuickMenu.SetActive(false);        
    }
    private void OnEnable()
    {
        UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed += HandleQuickMenuPressed;
        UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled += HandleQuickMenuReleased;
    }
    private void OnDisable()
    {
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed -= HandleQuickMenuPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled -= HandleQuickMenuReleased;
        }        
    }

    private new void OnDestroy()
    {
        base.OnDestroy();   
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed -= HandleQuickMenuPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled -= HandleQuickMenuReleased;
        }    
    }   

    private void HandleQuickMenuReleased(InputAction.CallbackContext obj)
    {
        QuickMenu.SetActive(false);
    }

     
    private void HandleQuickMenuPressed(InputAction.CallbackContext context)
    {
        QuickMenu.SetActive(true);
    }

}
