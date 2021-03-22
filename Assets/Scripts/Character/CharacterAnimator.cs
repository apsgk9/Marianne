using System;
using System.Collections.Generic;
using CharacterInput;
using UnityEngine;
using static LocomotionEnmus;

namespace CharacterProperties
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Animator Animator;
        public CharacterState CharacterState;
        private float _compositeSpeedValue;
        private Vector2 _rawDirection;
        public string SpeedParameterName = "Speed";
        public string MovementPressedParameterName ="MovementPressed";
        public string UsingControllerParameterName ="UsingController";
        public string ControllerDeltaParameterName ="ControllerDelta";
        public string CharacterHasStaminaParameterName ="HasStamina";
        public string JumpTriggerParameterName ="Jump";
        public string IsJumpingParameterName ="IsJumping";
        public string isGroundedParameterName ="isGrounded";
        public string InterruptableParameterName ="Interruptable";
        private static Vector2 _previousMovmementAxis;
        private ICharacterInput _characterInput;

        #region SwitchStatuses
        private bool _currentJumpButtonStatus;
        private bool _canJump;


        private LocomotionMode LocomotionMode;
        
        private bool _currentDashButtonStatus;
        private bool _dashbuttonIsLifted;
        private bool canDash;

        #endregion

        private void Awake()
        {
            _canJump=true;
            if(_characterInput==null)
            {
                _characterInput = GetComponentInParent<ICharacterInput>();
            }
            _previousMovmementAxis=Vector2.zero;

        }
        public void Update()
        {
            SpeedCalculations();
            SetParameters();
            HandleJump();
        }

        private void SetParameters()
        {
            Animator.SetFloat(SpeedParameterName, _compositeSpeedValue);

            Animator.SetBool(MovementPressedParameterName, GetMovementPressed());

            Animator.SetFloat(ControllerDeltaParameterName, ReturnDeltaMovement());

            Animator.SetBool(UsingControllerParameterName, InputHelper.DeviceInputTool.IsUsingController());

            Animator.SetBool(CharacterHasStaminaParameterName, CharacterState.CanUseStamina);

            Animator.SetBool(isGroundedParameterName, CharacterState.isGrounded);
        }

        private void HandleJump()
        {
            if(CharacterState.TryingToJump && CharacterState.CanJump)
            {
                Animator.SetTrigger(JumpTriggerParameterName);
            }
        }

        private bool GetMovementPressed()
        {
            return _characterInput.IsThereMovement();
        }

        private float ReturnDeltaMovement()
        {
            var movementAxis=new Vector2(_characterInput.MovementHorizontal(), _characterInput.MovementVertical());
            float delta= (movementAxis-_previousMovmementAxis).magnitude;     

            _previousMovmementAxis=movementAxis;
            return delta;
        }

        //Notused

        private bool ChangeInVelocity()
        {
            return !(CharacterState.DesiredDeltaVelocity.magnitude<Mathf.Epsilon);
        }

        private void SpeedCalculations()
        {
            //Calculates what speed input to put into the animator

            _currentDashButtonStatus = CharacterState.CharacterAnimatorSpeed > (float)LocomotionMode.Sprint - 0.001 &&
                           CharacterState.CharacterAnimatorSpeed < (float)LocomotionMode.Sprint + 0.001;
            if(_currentDashButtonStatus==false) //released/up
            {
                _dashbuttonIsLifted=true;
                canDash=true;
            }

            if(!_dashbuttonIsLifted)
            {
                //Prevent Dashing until button is lifted again
                if(CharacterState.CanUseStamina==false || Animator.GetBool(InterruptableParameterName))
                {
                    canDash=false;
                }

                _compositeSpeedValue = (CharacterState.CharacterAnimatorSpeed>(float)LocomotionMode.Run && !canDash)?
                (float)LocomotionMode.Run:
                CharacterState.CharacterAnimatorSpeed;
            }
            else
            {
                _compositeSpeedValue=CharacterState.CharacterAnimatorSpeed;
                _dashbuttonIsLifted=false;
            }            
        }


        private void OnValidate()
        {
            if(Animator==null)
            {
                Debug.LogWarning("PlayerAnimator must have an animator.");
            }
            if(CharacterState==null)
            {
                Debug.LogWarning("PlayerState is missing for PlayerAnimator.");
            }
        }
    }
}
