  
using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLookZoom))]
public class CinemachineFreeLookZoomController : MonoBehaviour
{
    private CinemachineFreeLookZoom freelookZoom;
    private InputSettings _InputSettings;

    private void Awake()
    {
        freelookZoom = GetComponentInChildren<CinemachineFreeLookZoom>();        
    }

    private IEnumerator Start()
    {
        
        while(!Service.ServiceLocator.Current.Exists<SettingsManager>())
            yield return null;
        _InputSettings= Service.ServiceLocator.Current.Get<SettingsManager>().GetInputSettings();
    }



    // Update is called once per frame
    void Update()
    {
        freelookZoom.Value=0f;
        if(GameStateMachine.Instance.isGameplay())
        {
            UpdateDesktopValues();
        }
    }

    private void UpdateDesktopValues()
    {
        freelookZoom.Value+=UserInput.Instance.Scroll;
    }
    public void SetZoomValue(float value)
    {
        freelookZoom.Value+=value;
    }

    public void SetZoomValueWithControllerSensititivity(float value)
    {
        freelookZoom.Value+=value*_InputSettings.ControllerZoomSensitivity;
    }
}