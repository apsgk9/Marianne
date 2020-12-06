using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterControllerCheckGrounded : MonoBehaviour, ICheckGrounded
{
    public float DistanceToGround = 0.1f;
    public Transform Origin;
    private CharacterController _CharacterController;
    public bool isGrounded{get;private set;}

    public bool _isGrounded;

    public Vector3 Scale = new Vector3(1f, 1f, 1f);
    public float radius = 0.1f;
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    //public LayerMask Mask;

    public event Action<bool> OnGroundedChange;
    public RaycastHit m_Hit;
    private bool m_HitDetect;
    public LayerMask Ground;

    private void Start()
    {
        _CharacterController = GetComponent<CharacterController>();
        isGrounded = true;
    }
    private void Update()
    {
        CheckGround();
        
    }
    private void FixedUpdate()
    {
        CheckGround();
    }


    private void CheckGround()
    {
        //Debug.DrawLine(Origin.position,Origin.position+(-Vector3.up*DistanceToGround),Color.red);
        //bool tempisGrounded = Physics.Raycast(Origin.position, -Vector3.up, DistanceToGround);

        bool tempisGrounded = Physics.BoxCast(transform.position + Offset, Scale, Vector3.down, out m_Hit, transform.rotation, DistanceToGround);
        //bool tempisGrounded = Physics.SphereCast(transform.position + Offset,radius,Vector3.down,out m_Hit,DistanceToGround, Ground, QueryTriggerInteraction.Ignore);
        bool grounded=(tempisGrounded||_CharacterController.isGrounded);
        if (isGrounded != grounded)
        {
            OnGroundedChange?.Invoke(grounded);
        }
        isGrounded = grounded;

        _isGrounded=isGrounded;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (isGrounded)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position + Offset, Vector3.down * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            //Gizmos.DrawSphere(transform.position + Offset + Vector3.down * m_Hit.distance, radius);
            Gizmos.DrawWireCube(transform.position + Offset + Vector3.down * m_Hit.distance, Scale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position + Offset, Vector3.down * DistanceToGround);
            //Draw a cube at the maximum distance
            //Gizmos.DrawSphere(transform.position + Offset + Vector3.down * DistanceToGround, radius);
            Gizmos.DrawWireCube(transform.position + Offset + Vector3.down * m_Hit.distance, Scale);
        }
    }
}
