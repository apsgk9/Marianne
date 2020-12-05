using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ICheckGrounded))]
public class PlayerCharacterControllerMover: MonoBehaviour, ICharacterMover
{
    public CharacterController _CharacterController { get; private set; }

    private ICheckGrounded _CheckGrounded;
    private bool _useGravity=true;
    public bool UseGravity { get {return _useGravity;} set {_useGravity=value;} }
    
    private Vector3 _totalVectorLateUpdate;
    
    private Vector3 _totalDisplacement;
    public Vector3 TotalVector { get => _totalDisplacement; set => _totalDisplacement=value; }

    public Vector3 Velocity;

    public float TerminalVelocity=55.56f;

    private void Awake()
    {
        _CharacterController= GetComponent<CharacterController>();
        _CheckGrounded= GetComponent<ICheckGrounded>();
        UseGravity=true;
        
    }
    public void Move(Vector3 motion)
    {
        _totalDisplacement+=motion;
    }
    private void ProcessGravity()
    {
        if (_CheckGrounded.isGrounded && Velocity.y<0)
        {          
            Velocity.y=0f;
        }
        Velocity.y+= Physics.gravity.y*Time.deltaTime;
        Velocity.y=Mathf.Clamp(Velocity.y,-TerminalVelocity,TerminalVelocity);
    }

    private void Update()
    {
        if(_useGravity)
        {
            ProcessGravity();
        }
        _CharacterController.Move(_totalDisplacement+Velocity*Time.deltaTime);
        _totalDisplacement=Vector3.zero;
    }

    public void AddExtraMotion(Vector3 motion)
    {
        _totalDisplacement+=motion;
    }
}
