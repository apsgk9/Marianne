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
    
    private Vector3 _totalDisplacement;
    public Vector3 TotalVector { get => _totalDisplacement; set => _totalDisplacement=value; }
    public float GravityMultiplier=1f;

    public Vector3 Drag= new Vector3(1,1,1);

    public Vector3 Velocity;
    private float initialradius;
    private float newRadius;

    private void Awake()
    {
        _CharacterController= GetComponent<CharacterController>();
        _CheckGrounded= GetComponent<ICheckGrounded>();
        initialradius=_CharacterController.radius;
        UseGravity=true;
        newRadius=initialradius*0.8f;
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
        Velocity.y+= Physics.gravity.y*Time.deltaTime*GravityMultiplier;
    }
    private void Update()
    {
        if (_useGravity)
        {
            ProcessGravity();
        }

        Velocity.x /= 1 + Drag.x * Time.deltaTime;
        Velocity.y /= 1 + Drag.y * Time.deltaTime;
        Velocity.z /= 1 + Drag.z * Time.deltaTime;
        _CharacterController.Move(Velocity * Time.deltaTime);
        HandleIfCharacterIsStuckOnLedge();
    }

    private void HandleIfCharacterIsStuckOnLedge()
    {
        if (!_CheckGrounded.isGrounded)
        {
            //_CharacterController.radius =Mathf.Lerp(_CharacterController.radius,newRadius,Time.deltaTime*40);
            //_CharacterController.skinWidth =Mathf.Lerp(_CharacterController.skinWidth,newRadius*0.1f,Time.deltaTime*40);
            _CharacterController.radius =newRadius;
            _CharacterController.skinWidth =_CharacterController.radius*0.15f;
        }
        else
        {
            
            //_CharacterController.radius =Mathf.Lerp(_CharacterController.radius,initialradius,Time.deltaTime*40);
            //_CharacterController.skinWidth =Mathf.Lerp(_CharacterController.skinWidth,initialradius*0.1f,Time.deltaTime*40);
            _CharacterController.radius = initialradius;
            _CharacterController.skinWidth =_CharacterController.radius*0.1f;
        }
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
    
    public void Jump(float height)
    {
        Velocity.y += Mathf.Sqrt(height * -2f * Physics.gravity.y);
    }

    public void SetVelocity(Vector3 vInput)
    {
        Velocity=vInput;
    }
}
