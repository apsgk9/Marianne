using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ICheckGrounded))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterRigidbodyMover : MonoBehaviour,ICharacterMover
{
    private ICheckGrounded _CheckGrounded;
    private Rigidbody _Rigidbody;

    public bool UseGravity { get {return _Rigidbody.useGravity;} set{_Rigidbody.useGravity=value;} }
    public Vector3 TotalVector { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Vector3 TotalMove;

    private void Awake()
    {
        _CheckGrounded= GetComponent<ICheckGrounded>();
        _Rigidbody= GetComponent<Rigidbody>();
    }


    public void AddVelocity(Vector3 vInput)
    {
        _Rigidbody.velocity+=vInput;
    }

    public void Move(Vector3 motion)
    {
        _Rigidbody.MovePosition((transform.position+motion));
    }

    public void SetVelocity(Vector3 vInput)
    {
        _Rigidbody.velocity=vInput;
    }
    public void AddExtraMotion(Vector3 motion)
    {
        throw new System.NotImplementedException();
    }

    public void Jump(float height)
    {
        throw new System.NotImplementedException();
    }
}
