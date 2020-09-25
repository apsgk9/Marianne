using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _tilt;
    [SerializeField]
    private float range=90f;

    private void Update()
    {
        //if(Pause.Active)
        //{
        //    return;
        //}
        float mouseRotation = Input.GetAxis("Mouse Y");
        _tilt = Mathf.Clamp(_tilt - mouseRotation, -range,range);
        transform.localRotation= Quaternion.Euler(_tilt,0f,0f);

    }    
}
