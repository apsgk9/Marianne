using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLookZoom))]
public class CinemachineFreeLookZoomController : MonoBehaviour
{
    private CinemachineFreeLookZoom freelookZoom;

    private void Awake()
    {
        freelookZoom = GetComponentInChildren<CinemachineFreeLookZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        freelookZoom.Value=0f;
        UpdateDesktopValues();
    }

    private void UpdateDesktopValues()
    {
        freelookZoom.Value+=UserInput.Instance.Scroll;
    }
}
