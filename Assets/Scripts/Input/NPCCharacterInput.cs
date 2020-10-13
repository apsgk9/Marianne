using System;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterInput
{
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCCharacterInput : MonoBehaviour, ICharacterInput
    {
        private NavMeshAgent agent;
        public float horizontal;
        public float vertical;
        public Vector3 DesiredDirection;
        public bool isRunning=false;
        [SerializeField] private float threshold=0.0001f;

        private void Awake()
        {
            agent=GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            
            CalculateVariables();            
            //agent.nextPosition=transform.position;
        }

        private void CalculateVariables()
        {
            if(agent.stoppingDistance >= Vector3.Distance(transform.position,agent.destination))
            {
                horizontal=0f;
                vertical=0f;
                DesiredDirection=Vector3.zero;
            }
            else if(agent.hasPath)
            {
                var firstPoint=  agent.path.corners[1];
                DesiredDirection =firstPoint-transform.position;
                horizontal=DesiredDirection.x;
                vertical=DesiredDirection.z;
            }
        }

        public float MovementHorizontal()
        {
            return horizontal;
        }

        public float MovementVertical()
        {
            return vertical;
        }
        public bool IsRunning()
        {
            return isRunning;
        }

        public bool IsThereMovement()
        {
            return Mathf.Abs(vertical)>Mathf.Epsilon || Mathf.Abs(horizontal)>Mathf.Epsilon;
        }
    }
}