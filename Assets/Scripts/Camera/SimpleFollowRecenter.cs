using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
using System;

public class SimpleFollowRecenter : MonoBehaviour
{
    public bool recenter;
    public float recenterTime = 0.5f;
    CinemachineFreeLook FreeLookVirtualCam;
    void Start()
    {
        FreeLookVirtualCam = GetComponent<CinemachineFreeLook>();
        FixTimeRecentering();
    }
    void Update()
    {
        Transform target = FreeLookVirtualCam != null ? FreeLookVirtualCam.Follow : null;
        if (target == null)
            return;
        RecenterCheck();
        Recenter();
    }

    

    private void Recenter()
    {
        FreeLookVirtualCam.m_RecenterToTargetHeading.m_enabled=recenter;
        FreeLookVirtualCam.m_YAxisRecentering.m_enabled=recenter;
    }
    private void RecenterCheck()
    {
        recenter=PlayerInput.Instance.isPlayerLookIdle;
    }
    private void FixTimeRecentering()
    {
        FreeLookVirtualCam.m_RecenterToTargetHeading.m_RecenteringTime = recenterTime;
        FreeLookVirtualCam.m_YAxisRecentering.m_RecenteringTime = recenterTime;
        FreeLookVirtualCam.m_RecenterToTargetHeading.m_WaitTime = 0;
        FreeLookVirtualCam.m_YAxisRecentering.m_WaitTime = 0;
    }
    private void OnValidate()
    {
        FreeLookVirtualCam = GetComponent<CinemachineFreeLook>();
        FixTimeRecentering();        
    }
}