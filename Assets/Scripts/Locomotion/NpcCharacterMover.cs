using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcCharacterMover : MonoBehaviour, ICharacterMover
{
    private NavMeshAgent agent;

    public void Move(Vector3 motion)
    {
        transform.position+=motion;
    }

    private void Awake()
    {
        SetUpAgent();
    }

    private void SetUpAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = false;
        agent.velocity = Vector3.zero;
        agent.acceleration = 0f;
        agent.angularSpeed = 0f;
        agent.speed = 0f;
    }

    private void Update()
    {
        BindAgentToTransform();
    }

    private void BindAgentToTransform()
    {
        agent.nextPosition = transform.position;
    }
    private void OnValidate()
    {
        SetUpAgent();
    }
}
