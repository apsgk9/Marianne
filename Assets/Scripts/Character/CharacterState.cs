using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using static LocomotionEnmus;

namespace CharacterProperties
{

    //What the character wants to do instead of what's happening
    public class CharacterState : MonoBehaviour, ICharacterState
    {
        public Vector3 DesiredVelocity { get; private set; }
        public Vector3 DesiredDeltaVelocity { get; private set; }
        public Vector3 ActualCurrentVelocity { get; private set; }
        public Vector3 _ActualCurrentVelocity;
        public float DesiredMagnitudeSpeed { get; private set; }
        public float CharacterAnimatorSpeed { get; private set; }
        public Character _player;
        public ICharacterStamina _staminaHandler;
        public IGroundSensors _GroundSensor;
        private bool _currentJumpButtonStatus;

        public bool CanUseStamina { get { return _staminaHandler.CanUse(); } }

        public bool TryingToJump { get; private set; }
        public bool CanJump { get; private set; }
        //public bool IsJumping { get; private set; }

        public bool isGrounded { get; private set; }

        public State State { get { return _state; } }
        private State _state=State.Falling;

        private void Awake()
        {
            //prevent from starting fall in the beginning
            isGrounded = true;

            DesiredVelocity = Vector3.zero;
            _player = GetComponent<Character>();
            _staminaHandler = GetComponentInChildren<ICharacterStamina>();
            _GroundSensor = GetComponent<IGroundSensors>();
        }
        private void FixedUpdate()
        {
            _ActualCurrentVelocity = ActualCurrentVelocity;
            //CalculateCanJump();            
        }


        private void OnEnable()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnDesiredMoveChange += UpdateDesiredVector;
                _player._Locomotion.OnMoveAnimatorSpeedChange += UpdateCharacterSpeed;
                _player._Locomotion.OnTryingToJump += UpdateTryingToJump;
                _player._Locomotion.OnCanJump += UpdateCanJump;
                _player._Locomotion.OnStateChange += UpdateState;

                _GroundSensor.OnGroundedChange += UpdateGrounded;
            }

        }



        /*
private void CalculateCanJump()
{
   Debug.Log(TryingToJump);
   if(TryingToJump==false) //released/up
   {
       _canJump=true;       
   }
   else 
   {
       if(_canJump)
       {
           _canJump=false;
       }
   }
}
*/



        private void Start()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnDesiredMoveChange -= UpdateDesiredVector;
                _player._Locomotion.OnDesiredMoveChange += UpdateDesiredVector;


                _player._Locomotion.OnMoveAnimatorSpeedChange -= UpdateCharacterSpeed;
                _player._Locomotion.OnMoveAnimatorSpeedChange += UpdateCharacterSpeed;


                _player._Locomotion.OnTryingToJump -= UpdateTryingToJump;
                _player._Locomotion.OnTryingToJump += UpdateTryingToJump;


                _GroundSensor.OnGroundedChange -= UpdateGrounded;
                _GroundSensor.OnGroundedChange += UpdateGrounded;


                _player._Locomotion.OnCanJump -= UpdateCanJump;
                _player._Locomotion.OnCanJump += UpdateCanJump;

            }
        }

        public void HasJumped()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            if (_player._Locomotion != null)
            {
                _player._Locomotion.OnDesiredMoveChange -= UpdateDesiredVector;
                _player._Locomotion.OnMoveAnimatorSpeedChange -= UpdateCharacterSpeed;
                _player._Locomotion.OnTryingToJump -= UpdateTryingToJump;
                _GroundSensor.OnGroundedChange -= UpdateGrounded;

                _player._Locomotion.OnCanJump -= UpdateCanJump;
            }

        }

        //private void CalculateCanJump()
        //{
        //    Debug.Log(TryingToJump);
        //    if(TryingToJump==false) //released/up
        //    {
        //        _canJump=true;       
        //    }
        //    else 
        //    {
        //        if(_canJump)
        //        {
        //            _canJump=false;
        //        }
        //    }
        //}

        #region Handlers
        private void UpdateDesiredVector(Vector3 MoveVector)
        {
            DesiredDeltaVelocity = MoveVector - DesiredVelocity;
            DesiredVelocity = MoveVector;
            DesiredMagnitudeSpeed = MoveVector.magnitude;
        }
        private void UpdateCharacterSpeed(float InputSpeed)
        {
            CharacterAnimatorSpeed = InputSpeed;
        }

        private void UpdateTryingToJump(bool JumpGiven)
        {
            TryingToJump = JumpGiven;
        }
        private void UpdateGrounded(bool inputIsGrounded)
        {
            isGrounded = inputIsGrounded;
        }

        private void UpdateCanJump(bool CanJumpGiven)
        {
            CanJump = CanJumpGiven;
        }

        private void UpdateState(State inputState)
        {
            _state= inputState;
            Debug.Log("STATE: "+inputState.ToString());
        }
        #endregion



    }
}