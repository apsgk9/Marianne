using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CheckGrounded : MonoBehaviour
{
    public float DistanceToGround;
    public Transform Origin;
    public bool isGrounded;//{ get;private set;}
    
    public event Action<bool> OnGroundedChange;

    private void Start()
    {
        isGrounded=true;
    }
    private void FixedUpdate()
    {
        CheckGround();
    }


    private void CheckGround()
    {
        //Debug.DrawLine(Origin.position,Origin.position+(-Vector3.up),Color.red);
        bool tempisGrounded = Physics.Raycast(Origin.position, -Vector3.up, DistanceToGround + 0.1f);

        if (isGrounded != tempisGrounded)
        {
            OnGroundedChange?.Invoke(tempisGrounded);
        }
        isGrounded = tempisGrounded;
    }
}
