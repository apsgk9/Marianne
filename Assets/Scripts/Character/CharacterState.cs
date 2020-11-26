using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterProperties
{
    public interface ICharacterState
    {
        Vector3 DesiredVelocity { get; }
        Vector3 DesiredDeltaVelocity { get; }
        float DesiredSpeed { get; }
        float AnimatorSpeed { get; }
        bool CanUseStamina { get; }
    }

    //What the character wants to do instead of what's happening
    public class CharacterState : MonoBehaviour, ICharacterState
    {
        public Vector3 DesiredVelocity { get; private set; }
        public Vector3 DesiredDeltaVelocity { get; private set; }
        public float DesiredSpeed { get; private set; }
        public float AnimatorSpeed { get; private set; }
        public Character _player;
        public ICharacterStamina _staminaHandler;
        public bool CanUseStamina { get { return _staminaHandler.CanUse(); }}

        private void Awake()
        {
            DesiredVelocity = Vector3.zero;
            _player = GetComponent<Character>();
            _staminaHandler = GetComponentInChildren<ICharacterStamina>();
        }
        private void OnEnable()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnMoveChange += UpdateVector;
                _player._Locomotion.OnMoveAnimatorSpeedChange += UpdateAnimatorSpeed;
            }

        }
        private void Start()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnMoveChange -= UpdateVector;
                _player._Locomotion.OnMoveChange += UpdateVector;


                _player._Locomotion.OnMoveAnimatorSpeedChange -= UpdateAnimatorSpeed;
                _player._Locomotion.OnMoveAnimatorSpeedChange += UpdateAnimatorSpeed;
            }
        }
        private void OnDisable()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnMoveChange -= UpdateVector;
                _player._Locomotion.OnMoveAnimatorSpeedChange -= UpdateAnimatorSpeed;
            }

        }

        private void UpdateVector(Vector3 MoveVector)
        {
            DesiredDeltaVelocity = MoveVector - DesiredVelocity;
            DesiredVelocity = MoveVector;
            DesiredSpeed = MoveVector.magnitude;
        }
        private void UpdateAnimatorSpeed(float InputSpeed)
        {
            AnimatorSpeed = InputSpeed;
        }
    }
}