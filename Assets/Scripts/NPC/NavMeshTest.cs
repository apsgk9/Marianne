using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [SerializeField]private const float StoppingDistance = 1f;
    [SerializeField] public Transform target;
    private NavMeshAgent agent;
    private RootMotionDelta _RootMotionDelta;
    private float previousTime;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _RootMotionDelta = GetComponentInChildren<RootMotionDelta>();

        //_RootMotionDelta.OnRootMotionChange += HandleRootMotionChange;

        previousTime=0f;
    }

    private void HandleRootMotionChange(Vector3 DeltaVector, Quaternion NewRotation)
    {
        var newPosition=DeltaVector+transform.position;
        agent.velocity=DeltaVector/Time.deltaTime;
        agent.nextPosition=newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //CalculateAnimator();
        if(Input.GetKeyDown(KeyCode.P))
        {
            SetDestination();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            agent.updatePosition=true;
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            agent.velocity=Vector3.zero;
        }
    }

    [ContextMenu("Set Destination")]
    void SetDestination()
    {        
        agent.SetDestination(target.position);
    }
}
