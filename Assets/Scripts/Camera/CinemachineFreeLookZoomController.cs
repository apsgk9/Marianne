using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLookZoom))]
public class CinemachineFreeLookZoomController : MonoBehaviour
{
    private CinemachineFreeLookZoom freelookZoom;

    private UIManager UIManager;

    private void Awake()
    {
        freelookZoom = GetComponentInChildren<CinemachineFreeLookZoom>();
       // UIManager= Service.ServiceLocator.Current.Get<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        freelookZoom.Value=0f;
        if(!UIManager.Instance.isInMenu)
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
}
