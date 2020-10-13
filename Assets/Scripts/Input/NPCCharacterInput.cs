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

        private void Awake()
        {
            agent=GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            
            CalculateVariables();            
            agent.nextPosition=transform.position;
        }

        private void CalculateVariables()
        {
            //if(!(agent.nextPosition==transform.position))
            //{
            //    DesiredDirection= agent.nextPosition;
            //    var temp=transform.TransformDirection(agent.nextPosition);
            //    horizontal=temp.x;
            //    vertical=temp.z;
            //}
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
            return false;
        }
    }
}