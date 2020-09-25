using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RecenterToPlayerForward : MonoBehaviour
{
    [SerializeField] KeyCode RecenterButton = KeyCode.T;
    bool isRecentering;
    public float RecenterTime;

    public CinemachineFreeLook FreeLookPlayerVirtualCam { get; private set; }

    void Start()
    {
        FreeLookPlayerVirtualCam = GetComponent<CinemachineFreeLook>();
        isRecentering=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(RecenterButton) && isRecentering==false)
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
        isRecentering=true;
        var PlayerRotation= GameObject.FindObjectOfType<Player>().transform.rotation;
        var LastYRotation= PlayerRotation.eulerAngles.y;

        var initialYAxis=FreeLookPlayerVirtualCam.m_YAxis.Value;
        var initialXAxis=FreeLookPlayerVirtualCam.m_XAxis.Value;

        float minVal = FreeLookPlayerVirtualCam.m_XAxis.m_MinValue;
        float maxValue = FreeLookPlayerVirtualCam.m_XAxis.m_MaxValue;

        var currentTime = 0f;
        while( currentTime <= RecenterTime && isRecentering==true)
        {
            var newTime = currentTime / RecenterTime;
            FreeLookPlayerVirtualCam.m_YAxis.Value = Mathf.SmoothStep(initialYAxis, 0.5f, newTime);

            //var newRotationX=Mathf.SmoothStep(initialXAxis,LastYRotation,newTime);
            //Debug.Log("newRotationX:"+newRotationX);
            FreeLookPlayerVirtualCam.m_XAxis.Value = 
            SmoothStepWrap(initialXAxis, LastYRotation,minVal,maxValue,newTime);

            //FreeLookPlayerVirtualCam.m_XAxis.Value=Mathf.LerpAngle(initialXAxis,LastYRotation,newTime);
            yield return null;
            currentTime += Time.deltaTime;
        }

        if (currentTime >= RecenterTime && isRecentering==true)
        {
            FreeLookPlayerVirtualCam.m_YAxis.Value=0.5f;
            FreeLookPlayerVirtualCam.m_XAxis.Value=LastYRotation;
        }
        isRecentering=false;
        
    }

    private float SmoothStepWrap(float from,float to,float min,float max, float t)
    {
        float difference=Mathf.Abs(to-from);
        if(difference<=180f)
        {
            return Mathf.SmoothStep(from, to, t);
        }
        else if(difference==360)
        {            
            return to;            
        }
        float toMax= Mathf.Abs(max-from);
        float toMin= Mathf.Abs(min-from);
        bool WrapAroundMax= toMax<toMin? true: false;
        difference= Mathf.Abs((max-min)-difference);
        
        if(WrapAroundMax)
        {
            float newStep= Mathf.SmoothStep(from, from+difference,t);
            float toreturn=newStep;
            if(newStep>max)
            {
                toreturn= newStep-max;
            }
            return toreturn;
        }
        else
        {
            float newStep= Mathf.SmoothStep(from, from-difference,t);
            float toreturn=newStep;
            if(newStep<min)
            {
                toreturn= max-newStep;
            }
            return toreturn;
        }
    }
}
