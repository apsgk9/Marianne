using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIQuickMenu : Singleton<UIQuickMenu>
{
    public GameObject QuickMenu;
    public GameEvent ZoomIn;
    public GameEvent ZoomOut;
    public bool zoomingIN;
    public bool zoomingOut;
    private void Awake()
    {
        QuickMenu.SetActive(false);        
    }
    private void OnEnable()
    {
        UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed += HandleQuickMenuPressed;
        UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled += HandleQuickMenuReleased;

        UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.performed += HandleZoomInPressed;
        UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.canceled += HandleZoomInReleased;

        UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.performed += HandleZoomOutPressed;
        UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.canceled += HandleZoomOutReleased;
    }

    

    private void OnDisable()
    {
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed -= HandleQuickMenuPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled -= HandleQuickMenuReleased;

            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.performed -= HandleZoomInPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.canceled -= HandleZoomInReleased;

            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.performed -= HandleZoomOutPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.canceled -= HandleZoomOutReleased;
        }        
    }

    private new void OnDestroy()
    {
        base.OnDestroy();   
        if(UserInput.Instance)
        {
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.performed -= HandleQuickMenuPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.QuickMenuKey.canceled -= HandleQuickMenuReleased;

            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.performed -= HandleZoomInPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomIn.canceled -= HandleZoomInReleased;

            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.performed -= HandleZoomOutPressed;
            UserInput.Instance.PlayerInputActions.PlayerControls.GamePadZoomOut.canceled -= HandleZoomOutReleased;
        }
    }

    private void Update()
    {
        if(QuickMenu.activeInHierarchy)
        {            
            if(zoomingIN)
            {
                ZoomIn.Raise();
            }
            else if(zoomingOut)
            {
                ZoomOut.Raise();
            }
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

    private void HandleZoomInReleased(InputAction.CallbackContext obj)
    {
        zoomingIN=false;
    }

    private void HandleZoomInPressed(InputAction.CallbackContext obj)
    {
        if(zoomingOut)
        {
            zoomingOut=false;
        }
        zoomingIN=true;
    }

    private void HandleZoomOutReleased(InputAction.CallbackContext obj)
    {
        zoomingOut=false;
    }

    private void HandleZoomOutPressed(InputAction.CallbackContext obj)
    {
        if(zoomingIN)
        {
            zoomingIN=false;
        }
        zoomingOut=true;
    }
}
