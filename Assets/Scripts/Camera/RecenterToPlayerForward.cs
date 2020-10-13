using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RecenterToPlayerForward : MonoBehaviour
{
    bool isRecentering;
    public float RecenterTime;

    public CinemachineFreeLook FreeLookPlayerVirtualCam { get; private set; }
    
    private Cinemachine.CinemachineTransposer.BindingMode initialBindingMode;
    void Start()
    {
        FreeLookPlayerVirtualCam = GetComponent<CinemachineFreeLook>();
        isRecentering=false;
    }

    public void TryRecenter()
    {
        if (isRecentering == false)
        {
            StartCoroutine(Recenter());
        }
    }

    public void CancelRecentering()
    {
        isRecentering=false;
    }

    private IEnumerator Recenter()
    {
        if(FreeLookPlayerVirtualCam.m_BindingMode != Cinemachine.CinemachineTransposer.BindingMode.WorldSpace)
        {
            SetupBindingMode();
        }
        
        isRecentering =true;
        var PlayerRotation= FreeLookPlayerVirtualCam.LookAt.transform.rotation;
        var LastYRotation= FreeLookPlayerVirtualCam.LookAt.transform.rotation.eulerAngles.y;
        

        var initialYAxis=FreeLookPlayerVirtualCam.m_YAxis.Value;
        var initialXAxis=FreeLookPlayerVirtualCam.m_XAxis.Value;

        float minVal = FreeLookPlayerVirtualCam.m_XAxis.m_MinValue;
        float maxValue = FreeLookPlayerVirtualCam.m_XAxis.m_MaxValue;

        var currentTime = 0f;
        while( currentTime <= RecenterTime && isRecentering==true)
        {
            var newTime = currentTime / RecenterTime;
            FreeLookPlayerVirtualCam.m_YAxis.Value = Mathf.SmoothStep(initialYAxis, 0.5f, newTime);

            FreeLookPlayerVirtualCam.m_XAxis.Value = 
            SmoothStepWrap(initialXAxis, LastYRotation,minVal,maxValue,newTime);

            yield return null;
            currentTime += Time.deltaTime;
        }

        if (currentTime >= RecenterTime && isRecentering==true)
        {
            FreeLookPlayerVirtualCam.m_YAxis.Value=0.5f;
            FreeLookPlayerVirtualCam.m_XAxis.Value=LastYRotation;
        }
        CancelRecentering();
        
    }

    private void SetupBindingMode()
    {
        FreeLookPlayerVirtualCam.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.WorldSpace;
        FreeLookPlayerVirtualCam.m_XAxis.m_Wrap=true;
        FreeLookPlayerVirtualCam.m_XAxis.m_MinValue=0;
        FreeLookPlayerVirtualCam.m_XAxis.m_MaxValue=360;
    }

    private float SmoothStepWrap(float from,float to,float min,float max, float t)
    {
        
        float difference=Mathf.Abs(to-from);
        if(difference<=180f)
        {
            var result=Mathf.SmoothStep(from, to, t);
            return result;
        }
        else if(difference==360)
        {            
            return to;            
        }
        float toMax= Mathf.Abs(max-from);
        float toMin= Mathf.Abs(min-from);
        bool WrapStartAtMax= toMax<toMin? true: false;
        difference= Mathf.Abs((max-min)-difference);
        
        if(WrapStartAtMax)
        {
            float newStep= Mathf.SmoothStep(0, difference, t);
            float toreturn=from+newStep;
            //Debug.Log("toreturn before:"+toreturn+"||max:"+max+"||target:"+to);
            if(toreturn>max)
            {
                //Debug.Log("toreturn INITIAL:"+toreturn);
                toreturn= min+(toreturn-max);
                //Debug.Log("toreturn FINAL:"+toreturn);
            }
            return toreturn;
        }
        else
        {
            float newStep= Mathf.SmoothStep(0, difference, t);
            float toreturn=from-newStep;
            //Debug.Log("toreturn before:"+toreturn+"||min:"+min+"||target:"+to);
            if(toreturn<min)
            {
                //Debug.Log("toreturn INITIAL:"+toreturn);
                toreturn= max+(toreturn-min);
                //Debug.Log("toreturn FINAL:"+toreturn);
            }
            return toreturn;
        }
    }
}
