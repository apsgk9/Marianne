using UnityEngine;
using Cinemachine;



[RequireComponent(typeof(CinemachineFreeLook)), DisallowMultipleComponent]
public class FreeLookAxisDriver : MonoBehaviour
{
    public float SpeedMultiplier=0.0075f;
    public CinemachineInputAxisDriver xAxis;
    public CinemachineInputAxisDriver yAxis;

    private CinemachineFreeLook freeLook;

    private RecenterToPlayerForward RecenterToPlayerForward;

    public float RecenterThreshold= 1f;
    public bool CanCancelRecenter=false;

    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        freeLook.m_XAxis.m_MaxSpeed = freeLook.m_XAxis.m_AccelTime = freeLook.m_XAxis.m_DecelTime = 0;
        freeLook.m_XAxis.m_InputAxisName = string.Empty;
        freeLook.m_YAxis.m_MaxSpeed = freeLook.m_YAxis.m_AccelTime = freeLook.m_YAxis.m_DecelTime = 0;
        freeLook.m_YAxis.m_InputAxisName = string.Empty;
        RecenterToPlayerForward = GetComponent<RecenterToPlayerForward>();
        
    }

    private void OnValidate()
    {
        xAxis.Validate();
        yAxis.Validate();
        xAxis.speedMultiplier=SpeedMultiplier;
        yAxis.speedMultiplier=SpeedMultiplier;
        RecenterToPlayerForward = GetComponent<RecenterToPlayerForward>();
    }

    private void Reset()
    {
        SpeedMultiplier=0.0075f;
        xAxis = new CinemachineInputAxisDriver
        {
            multiplier = 5f,
            accelTime = 0.1f,
            decelTime = 0.1f,
            axisSpace = AxisSpace.x,
            speedMultiplier=SpeedMultiplier,
        };
        yAxis = new CinemachineInputAxisDriver
        {
            multiplier = -0.025f,
            accelTime = 0.1f,
            decelTime = 0.1f,
            axisSpace = AxisSpace.y,
            speedMultiplier=SpeedMultiplier,
        };
    }

    private void Update()
    {
        //bool changed = xAxis.Update(Time.deltaTime, ref freeLook.m_XAxis);
        //if (yAxis.Update(Time.deltaTime, ref freeLook.m_YAxis))
        //    changed = true;
        if (isCursorLocked())
        {
            float xAxisInput = xAxis.Update(Time.deltaTime, ref freeLook.m_XAxis);
            float yAxisInput = yAxis.Update(Time.deltaTime, ref freeLook.m_YAxis);
            if (CanCancelRecenter)
            {

                Vector2 axisInputs = new Vector2(xAxisInput, yAxisInput);
                if (axisInputs.magnitude > RecenterThreshold)
                {
                    freeLook.m_RecenterToTargetHeading.CancelRecentering();
                    freeLook.m_YAxisRecentering.CancelRecentering();
                    RecenterToPlayerForward.CancelRecentering();
                }
            }
        }
    }

    private static bool isCursorLocked()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
