using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
using System;
//https://forum.unity.com/threads/trouble-recentering-the-x-axis-of-the-free-look-camera.539097/
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

    private bool isOnCenter()
    {
        Transform target = FreeLookVirtualCam.Follow;

        // How far away from centered are we?
        Vector3 up = FreeLookVirtualCam.State.ReferenceUp;
        Vector3 back = FreeLookVirtualCam.transform.position - target.position;
        float angle = UnityVectorExtensions.SignedAngle(
            back.ProjectOntoPlane(up), -target.forward.ProjectOntoPlane(up), up);
        if (Mathf.Abs(angle) < 0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RecenterCheck()
    {
        recenter=PlayerInput.Instance.isPlayerLookIdle && !isOnCenter();
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