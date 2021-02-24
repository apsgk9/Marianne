using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(CharacterController))]
public class PlayerCheckGrounded : MonoBehaviour, ICheckGrounded
{
    private const float AirScale = 0.05f;
    public float DistanceToGround = 0.1f;
    [Range(0,1f)]
    public float InAirDistanceExtension = 0.3f;
    public List<Transform> OnGroundOrigins=new List<Transform>();
    public List<Transform> InAirOrigins=new List<Transform>();

    //private CharacterController _CharacterController;

    public bool isGrounded{get{return _isGrounded;}set{value=_isGrounded;}}
    public bool _isGrounded;
    public float DistancetoGround{get;private set;}

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
        bool groundedResults=false;
        float tempDistanceResults=0f;
        DistancetoGround=0f;
        if(_isGrounded)
        {
            foreach(var origin in OnGroundOrigins)
            {
                bool result=(groundedResults||Physics.Raycast(origin.position + Offset, Vector3.down, out m_Hit, DistanceToGround));               
                groundedResults=groundedResults||result;
                tempDistanceResults += (origin.position.y - m_Hit.point.y);
            }
        }
        else
        {
            foreach(var origin in InAirOrigins)
            {
                groundedResults=
                (groundedResults||Physics.Raycast(origin.position + Offset+origin.forward*AirScale, Vector3.down, out m_Hit, DistanceToGround+ InAirDistanceExtension))
                ||(groundedResults||Physics.Raycast(origin.position + Offset-origin.forward*AirScale, Vector3.down, out m_Hit, DistanceToGround+ InAirDistanceExtension))
                ||(groundedResults||Physics.Raycast(origin.position + Offset+origin.right*AirScale, Vector3.down, out m_Hit, DistanceToGround+ InAirDistanceExtension))
                ||(groundedResults||Physics.Raycast(origin.position + Offset-origin.right*AirScale, Vector3.down, out m_Hit, DistanceToGround+ InAirDistanceExtension));
            }            
        }
        

        //float Angle=Vector3.Angle(m_Hit.normal,transform.forward)-90;
        if (isGrounded != groundedResults)
        {
            OnGroundedChange?.Invoke(groundedResults);
        }
        _isGrounded=groundedResults;
        if(_isGrounded)
        {            
            DistancetoGround=(tempDistanceResults/OnGroundOrigins.Count);
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
            //Draw a Ray forward from GameObject toward the hit
            //Gizmos.DrawRay(transform.position + Offset, Vector3.down * m_Hit.distance);
            foreach(var origin in OnGroundOrigins)
            {
                Gizmos.DrawRay(origin.position + Offset, Vector3.down * m_Hit.distance);
            }
            
            
            //Draw a cube that extends to where the hit exists
            //Gizmos.DrawSphere(transform.position + Offset + Vector3.down * m_Hit.distance, radius);
            //Gizmos.DrawWireCube(transform.position + Offset + Vector3.down * m_Hit.distance, Scale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            
            //foreach(var origin in OnGroundOrigins)
            //{
            //    Gizmos.DrawRay(origin.position + Offset, Vector3.down * DistanceToGround);
            //}
            foreach(var origin in InAirOrigins)
            {
              Gizmos.DrawRay(origin.position + Offset+origin.forward* AirScale, Vector3.down * (DistanceToGround+InAirDistanceExtension));
              Gizmos.DrawRay(origin.position + Offset-origin.forward*AirScale, Vector3.down * (DistanceToGround+InAirDistanceExtension));
              Gizmos.DrawRay(origin.position + Offset+origin.right*AirScale, Vector3.down * (DistanceToGround+InAirDistanceExtension));
              Gizmos.DrawRay(origin.position + Offset-origin.right*AirScale, Vector3.down * (DistanceToGround+InAirDistanceExtension));
            }
            
        }
    }
}
