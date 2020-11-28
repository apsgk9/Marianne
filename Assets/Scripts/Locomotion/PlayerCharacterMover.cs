using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterMover: MonoBehaviour, ICharacterMover
{
    public CharacterController _CharacterController { get; private set; }
    private bool _useGravity=true;
    public bool UseGravity { get {return _useGravity;} set {_useGravity=value;} }

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
}
