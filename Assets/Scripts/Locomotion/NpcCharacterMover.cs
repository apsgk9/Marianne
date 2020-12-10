using System;
using System.Collections;
using System.Collections.Generic;
using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NPCCharacterInput))]
public class NpcCharacterMover : MonoBehaviour, ICharacterMover
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

    public void Move(Vector3 motion)
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
}
