using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(CharacterController))]
public class PlayerCheckGrounded : MonoBehaviour, ICheckGrounded
{
    public float DistanceToGround = 0.1f;
    public List<Transform> ListOfOrigins=new List<Transform>();

    //private CharacterController _CharacterController;

    public bool isGrounded{get{return _isGrounded;}set{value=_isGrounded;}}
    public bool _isGrounded;

    //public Vector3 Scale = new Vector3(1f, 1f, 1f);
    //public float radius = 0.1f;
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    //public LayerMask Mask;
    public event Action<bool> OnGroundedChange;
    public RaycastHit m_Hit;
    private bool m_HitDetect;
    public LayerMask Ground;
    private float stretchifGrounded;

    private void Start()
    {
        //_CharacterController = GetComponent<CharacterController>();
        isGrounded = true;
    }
    private void Update()
    {
        stretchifGrounded= (isGrounded)?2:1f;
        CheckGround();        
    }

    private void CheckGround()
    {
        //Debug.DrawLine(Origin.position,Origin.position+(-Vector3.up*DistanceToGround),Color.red);
        //bool tempisGrounded = Physics.Raycast(Origin.position, -Vector3.up, DistanceToGround);
        //bool tempisGrounded = Physics.BoxCast(transform.position + Offset, Scale, Vector3.down, out m_Hit, transform.rotation, DistanceToGround);
        bool groundedResults=false;
        foreach(var origin in ListOfOrigins)
        {
            groundedResults=(groundedResults||Physics.Raycast(origin.position + Offset, Vector3.down, out m_Hit, DistanceToGround*stretchifGrounded));
        }

        //float Angle=Vector3.Angle(m_Hit.normal,transform.forward)-90;
        if (isGrounded != groundedResults)
        {
            Debug.Log("groundedResults: "+groundedResults);
            OnGroundedChange?.Invoke(groundedResults);
        }
        _isGrounded=groundedResults;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (isGrounded)
        {
            //Draw a Ray forward from GameObject toward the hit
            //Gizmos.DrawRay(transform.position + Offset, Vector3.down * m_Hit.distance);
            foreach(var origin in ListOfOrigins)
            {
                Gizmos.DrawRay(origin.position + Offset, Vector3.down * m_Hit.distance*stretchifGrounded);
            }
            
            //Draw a cube that extends to where the hit exists
            //Gizmos.DrawSphere(transform.position + Offset + Vector3.down * m_Hit.distance, radius);
            //Gizmos.DrawWireCube(transform.position + Offset + Vector3.down * m_Hit.distance, Scale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            //Gizmos.DrawRay(transform.position + Offset, Vector3.down * DistanceToGround);
            foreach(var origin in ListOfOrigins)
            {
                Gizmos.DrawRay(origin.position + Offset, Vector3.down * DistanceToGround*stretchifGrounded);
            }
            
            //Draw a cube at the maximum distance
            //Gizmos.DrawSphere(transform.position + Offset + Vector3.down * DistanceToGround, radius);
            //Gizmos.DrawWireCube(transform.position + Offset + Vector3.down * m_Hit.distance, Scale);
        }
    }
}
