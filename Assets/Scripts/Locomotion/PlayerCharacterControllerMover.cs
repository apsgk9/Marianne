using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterControllerMover: MonoBehaviour, ICharacterMover
{
    public CharacterController _CharacterController { get; private set; }
    private bool _useGravity=true;
    public bool UseGravity { get {return _useGravity;} set {_useGravity=value;} }
    
    private Vector3 _totalVector;
    private Vector3 _total1Vector;
    public Vector3 TotalVector { get => _totalVector; set => _totalVector=value; }

    private void Awake()
    {
        _CharacterController= GetComponent<CharacterController>();
        UseGravity=true;
        
    }
    public void Move(Vector3 motion)
    {
        if(_useGravity)
        {
            motion = ApplyGravity(motion);
        }
        _CharacterController.Move(motion);
    }
    private Vector3 ApplyGravity(Vector3 motion)
    {
        return motion+Physics.gravity*Time.deltaTime;
    }
    private void LateUpdate()
    {
        _CharacterController.Move(TotalVector);
        TotalVector=Vector3.zero;
    }
    public void AddExtraMotion(Vector3 motion)
    {
        TotalVector+=motion;
    }
}
