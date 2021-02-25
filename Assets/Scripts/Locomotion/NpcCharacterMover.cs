using System;
using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

//Haven't been tested.

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NPCCharacterInput))]
public class NpcCharacterMover : MonoBehaviour, IMover
{
    private NavMeshAgent _agent;
    private CharacterController _CharacterController;
    private NPCCharacterInput _npcCharacterInput;
    private Vector3 nextposition;
    private bool _useGravity=true;
    public bool UseGravity { get {return _useGravity;} set {_useGravity=value;} }

    public Vector3 _finalVector;
    
    public Vector3 TotalVector { get => _finalVector; set => _finalVector=value; }
    public float GravityMultiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private void Awake()
    {
        SetUp();
        _agent.isStopped=true;
    }

    private void SetUp()
    {
        _CharacterController = GetComponent<CharacterController>();
        _npcCharacterInput = GetComponent<NPCCharacterInput>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updatePosition = false;
        _agent.velocity = Vector3.zero;
        _agent.acceleration = 0f;
        _agent.angularSpeed = 0f;
        _agent.speed = 0f;
    }

    public void OnUpdateAnimatorMove(Vector3 motion)
    {
        if(_useGravity)
        {
            _finalVector = ApplyGravity(motion);
        }
        _finalVector=motion;
    }
    
    private Vector3 ApplyGravity(Vector3 motion)
    {
        return motion+Physics.gravity*Time.deltaTime;
    }
    

    private void Update()
    {        
        BindAgentToTransform();
        _CharacterController.Move(_finalVector);
    }

    private void BindAgentToTransform()
    {
        _agent.nextPosition = transform.position;
    }
    private void OnValidate()
    {
        SetUp();
    }

    public void AddExtraMotion(Vector3 motion)
    {
        _finalVector+=motion;
    }

    public void AddVelocity(Vector3 vInput)
    {
        throw new NotImplementedException();
    }

    public void SetVelocity(Vector3 vInput)
    {
        throw new NotImplementedException();
    }

    public void Jump(float height)
    {
        throw new NotImplementedException();
    }

    public void SetConstantMoveUpdate(Vector3 motion)
    {
        throw new NotImplementedException();
    }

    public void SetGroundVelocity(float x, float y, float z)
    {
        throw new NotImplementedException();
    }

    public void SetGroundVelocity(Vector2 vInput)
    {
        throw new NotImplementedException();
    }

    public void SetGroundVelocity(float x, float z)
    {
        throw new NotImplementedException();
    }

    public void CheckForGround()
    {
        throw new NotImplementedException();
    }

    public Collider GetGroundCollider()
    {
        throw new NotImplementedException();
    }

    public Vector3 GetGroundNormal()
    {
        throw new NotImplementedException();
    }

    public Vector3 GetGroundPoint()
    {
        throw new NotImplementedException();
    }

    public bool IsGrounded()
    {
        throw new NotImplementedException();
    }

    public void RecalculateColliderDimensions()
    {
        throw new NotImplementedException();
    }

    public void SetColliderHeight(float _newColliderHeight)
    {
        throw new NotImplementedException();
    }

    public void SetExtendSensorRange(bool _isExtended)
    {
        throw new NotImplementedException();
    }

    public void SetStepHeightRatio(float _newStepHeightRatio)
    {
        throw new NotImplementedException();
    }
}
