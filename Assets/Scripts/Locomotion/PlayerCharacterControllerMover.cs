using System;
using UnityEditor;
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
    public float JumpHeight=2f;

    public Vector3 Drag= new Vector3(1,1,1);

    public Vector3 Velocity;

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
    }
    private void Update()
    {
        if(_useGravity)
        {
            ProcessGravity();
        }

        Velocity.x /= 1 + Drag.x * Time.deltaTime;
        Velocity.y /= 1 + Drag.y * Time.deltaTime;
        Velocity.z /= 1 + Drag.z * Time.deltaTime;
        _CharacterController.Move(Velocity*Time.deltaTime);
    }

    private void LateUpdate()
    {
        _CharacterController.Move(_totalDisplacement);
        _totalDisplacement=Vector3.zero;
    }

    public void AddExtraMotion(Vector3 motion)
    {
        _totalDisplacement+=motion;
    }
    public void AddVelocity(Vector3 vInput)
    {
        Velocity+=vInput;
    }
    
    [ContextMenu("JUMP")]
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && _CheckGrounded.isGrounded)
            Velocity.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
    }
}
