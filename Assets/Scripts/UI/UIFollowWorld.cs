using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UIFollowWorld : MonoBehaviour
{

    [SerializeField] public Transform WorldTarget;
    [SerializeField] public Vector3 LocalOffset;
    public Camera PlayerCamera;
    void Start()
    {
        if(PlayerCamera==null)
        {
            PlayerCamera = Camera.main;
        }        
    }

    
    void OnGUI()
    {
        Vector3 pos= PlayerCamera.WorldToScreenPoint(WorldTarget.position);

        if(transform.position !=pos)
        {
            transform.position = pos;
            transform.localPosition+= LocalOffset;
        }
        
    }
}
