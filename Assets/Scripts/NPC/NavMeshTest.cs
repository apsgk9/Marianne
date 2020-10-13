using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [SerializeField] public Transform target;
    private NavMeshAgent agent;
    private float previousTime;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
