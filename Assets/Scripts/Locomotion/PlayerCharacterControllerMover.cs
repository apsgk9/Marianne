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
    public float SlipSpeed=2f;

    public float GravityMultiplier=1f;

    public Vector3 Drag= new Vector3(1,1,1);

    public Vector3 Velocity;
    private float initialradius;
    private float newRadius;
    private ControllerColliderHit ColliderHit;

    private float hitDistance;

    private void Awake()
    {
        _CharacterController= GetComponent<CharacterController>();
        _CheckGrounded= GetComponent<ICheckGrounded>();
        initialradius=_CharacterController.radius;
        UseGravity=true;
        newRadius=initialradius*0.5f;        
        _CharacterController.skinWidth =_CharacterController.radius*0.1f;
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
        else
        {
            Velocity.y+= Physics.gravity.y*Time.deltaTime*GravityMultiplier;
        }
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
        bool stuckonledge = !_CheckGrounded.isGrounded && _CharacterController.velocity == Vector3.zero && Velocity.y != 0f
        && (transform.position-ColliderHit.point).magnitude<=hitDistance+0.1f;
        if (stuckonledge)
        {
            Vector3 moveby=ColliderHit.normal;
            _CharacterController.Move(moveby*Time.deltaTime*SlipSpeed);
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

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        ColliderHit= hit;
        hitDistance=(transform.position-ColliderHit.point).magnitude;
    }
}
