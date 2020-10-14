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
    private CharacterController _characterController;
    private NPCCharacterInput _npcCharacterInput;
    private Vector3 nextposition;

    public void Move(Vector3 motion)
    {
        _characterController.Move(motion);
    }
    private void Awake()
    {
        SetUp();
        _agent.isStopped=true;
    }

    private void SetUp()
    {
        _characterController = GetComponent<CharacterController>();
        _npcCharacterInput = GetComponent<NPCCharacterInput>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updatePosition = false;
        _agent.velocity = Vector3.zero;
        _agent.acceleration = 0f;
        _agent.angularSpeed = 0f;
        _agent.speed = 0f;
    }

    private void Update()
    {        
        BindAgentToTransform();
    }

    private void BindAgentToTransform()
    {
        _agent.nextPosition = transform.position;
    }
    private void OnValidate()
    {
        SetUp();
    }

}
