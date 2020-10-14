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
        private MovementHistory _verticalHistory;
        private MovementHistory _horizontalHistory;
        private int historyMaxLength=4;

        private void Awake()
        {
            agent=GetComponent<NavMeshAgent>();
            _verticalHistory= new MovementHistory(historyMaxLength);
            _horizontalHistory= new MovementHistory(historyMaxLength);
        }
        private void Update()
        {

            CalculateVariables();
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
            _verticalHistory.Tick(vertical);
            _horizontalHistory.Tick(horizontal);
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
            float verticalAverage=_verticalHistory.Average();
            float horizontalAverage=_horizontalHistory.Average();
        
            bool isMovementhere= verticalAverage > 0.0025 || horizontalAverage > 0.0025;
            return isMovementhere;
        }
    }
}