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
        agent=GetComponent<NavMeshAgent>();
        agent.updateRotation=false;
        agent.updatePosition=false;
    }

}
