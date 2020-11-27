using System;
using System.Collections.Generic;
using CharacterInput;
using UnityEngine;

namespace CharacterProperties
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Animator Animator;
        public CharacterState CharacterState;
        public string SpeedParameterName = "Speed";
        private float _compositeSpeedValue;
        private Vector2 _rawDirection;
        public string MovementPressedParameterName ="MovementPressed";
        public string UsingControllerParameterName ="UsingController";
        public string ControllerDeltaParameterName ="ControllerDelta";
        public string CharacterHasStaminaParameterName ="HasStamina";
        public string JumpTriggerParameterName ="Jump";
        public string isGroundedParameterName ="isGrounded";
        private static Vector2 _previousMovmementAxis;
        private ICharacterInput _characterInput;

        private void Awake()
        {   
            if(_characterInput==null)
            {
                _characterInput = GetComponentInParent<ICharacterInput>();
            }
            _previousMovmementAxis=Vector2.zero;
        }
        public void Update()
        {
            SpeedCalculations();

            Animator.SetFloat(SpeedParameterName, _compositeSpeedValue);

            Animator.SetBool(MovementPressedParameterName, GetMovementPressed());

            Animator.SetFloat(ControllerDeltaParameterName, ReturnDeltaMovement());

            Animator.SetBool(UsingControllerParameterName, InputHelper.DeviceInputTool.IsUsingController());

            Animator.SetBool(CharacterHasStaminaParameterName, CharacterState.CanUseStamina);

            Animator.SetBool(isGroundedParameterName, CharacterState.isGrounded);

            HandleJump();
        }

        private void HandleJump()
        {
            bool currentJumpStatus = CharacterState.isJumping;
            if(currentJumpStatus==true)
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

        private bool ChangeInVelocity()
        {
            return !(CharacterState.DesiredDeltaVelocity.magnitude<Mathf.Epsilon);
        }

        private void SpeedCalculations()
        {
            //please clarify more of this
            _compositeSpeedValue=CharacterState.AnimatorSpeed;
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
