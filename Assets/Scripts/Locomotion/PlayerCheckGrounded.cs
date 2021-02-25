using System;
using System.Collections;
using System.Collections.Generic;
using CharacterProperties;
using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(CharacterController))]
public class PlayerCheckGrounded : MonoBehaviour, IGroundSensors
{
    private const float AirScale = 0.05f;
    public float MaxDistanceToGround = 0.1f;

    [Range(0,1f)]
    public float InAirDistanceExtension = 0.3f;

    public Transform Origin;

    //private CharacterController _CharacterController;
    public bool isGrounded{get{return _isGrounded;}set{value=_isGrounded;}}
    public bool _isGrounded;
    public float DistancetoGround{get;private set;}
    public float Angle { get; private set; }

    //public Vector3 Scale = new Vector3(1f, 1f, 1f);
    //public float radius = 0.1f;
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    //public LayerMask Mask;
    public event Action<bool> OnGroundedChange;
    public RaycastHit m_Hit;
    private bool m_HitDetect;
    public LayerMask Ground;
    private void Start()
    {
        //_CharacterController = GetComponent<CharacterController>();
        isGrounded = true;
    }
    private void Update()
    {
        CheckGround();        
    }

    private void CheckGround()
    {
        bool groundedResult=false;
        DistancetoGround=0f;

        if(isGrounded)
        {
            groundedResult=Physics.Raycast(Origin.position + Offset, Vector3.down, out m_Hit, MaxDistanceToGround);    
        }
        else
        {
            groundedResult=Physics.Raycast(Origin.position + Offset, Vector3.down, out m_Hit, MaxDistanceToGround+InAirDistanceExtension);    
        }
        
        Angle=Vector3.Angle(m_Hit.normal,transform.forward)-90;

        if (isGrounded != groundedResult)
        {
            OnGroundedChange?.Invoke(groundedResult);
        }
        _isGrounded=groundedResult;
        if(_isGrounded)
        {            
            DistancetoGround = (Origin.position.y - m_Hit.point.y);
            if(DistancetoGround<0)
                DistancetoGround=0f;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (isGrounded)
        {
            Gizmos.DrawRay(Origin.position + Offset, Vector3.down * m_Hit.distance);
            
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {            
            Gizmos.DrawRay(Origin.position + Offset, Vector3.down * MaxDistanceToGround);            
        }
    }
}
