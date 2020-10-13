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
            var tempDirection=agent.nextPosition-transform.position;
            if(tempDirection.magnitude>threshold)
            {
                DesiredDirection=  agent.nextPosition-transform.position;
                var temp=transform.TransformDirection(agent.nextPosition);
                //horizontal=temp.x;
                //vertical=temp.z;
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