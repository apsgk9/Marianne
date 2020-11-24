using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterProperties
{

    public class CharacterState : MonoBehaviour
    {
        public Vector3 Velocity;
        public Vector3 DeltaVelocity;
        public float Speed;
        public float AnimatorSpeed;
        public Character _player;

        private void Awake()
        {
            Velocity=Vector3.zero;
            _player = GetComponent<Character>();
        }
        private void OnEnable()
        {
            if( _player._Locomotion!=null)
            {
                 _player._Locomotion.OnMoveChange+= UpdateVector;
                 _player._Locomotion.OnMoveAnimatorSpeedChange+=UpdateAnimatorSpeed;
            }

        }
        private void Start()
        {
            if( _player._Locomotion!=null)
            {
                 _player._Locomotion.OnMoveChange-= UpdateVector;
                 _player._Locomotion.OnMoveChange+= UpdateVector;


                 _player._Locomotion.OnMoveAnimatorSpeedChange-=UpdateAnimatorSpeed;
                 _player._Locomotion.OnMoveAnimatorSpeedChange+=UpdateAnimatorSpeed;
            }        
        }
        private void OnDisable()
        {
            if( _player._Locomotion!=null)
            {
                 _player._Locomotion.OnMoveChange-= UpdateVector;
                 _player._Locomotion.OnMoveAnimatorSpeedChange-=UpdateAnimatorSpeed;
            }

        }

        private void UpdateVector(Vector3 MoveVector)
        {
            DeltaVelocity=MoveVector-Velocity;
            Velocity=MoveVector;
            Speed=MoveVector.magnitude;
        }
        private void UpdateAnimatorSpeed(float InputSpeed)
        {
            AnimatorSpeed=InputSpeed;
        }
    }
}