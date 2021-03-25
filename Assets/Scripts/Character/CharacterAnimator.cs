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

        [SerializeField]
        private CharacterAnimatorNamingList  CharacterAnimatorNamingList;
        private static Vector2 _previousMovmementAxis;
        private ICharacterInput _characterInput;

        #region SwitchStatuses
        private bool _currentJumpButtonStatus;
        private bool _canJump;


        private LocomotionMode LocomotionMode;
        
        private bool _currentDashButtonStatus;
        private bool _dashbuttonIsLifted;
        private bool canDash;

        
        private object _previousCurrentStateInfo;
        private object _previousNextStateInfo;
        private object _previousIsAnimatorTransitioning;
        private object _currentStateInfo;
        private object _nextStateInfo;
        private object _isAnimatorTransitioning;

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
        void FixedUpdate()
        {
            CacheAnimatorState();
        }

        // Called at the start of FixedUpdate to record the current state of the base layer of the animator.
        void CacheAnimatorState()
        {
            _previousCurrentStateInfo = _currentStateInfo;
            _previousNextStateInfo = _nextStateInfo;
            _previousIsAnimatorTransitioning = _isAnimatorTransitioning;

            _currentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            _nextStateInfo = Animator.GetNextAnimatorStateInfo(0);
            _isAnimatorTransitioning = Animator.IsInTransition(0);
        }

        private void SetParameters()
        {
            Animator.SetFloat(CharacterAnimatorNamingList.SpeedParameterName, _compositeSpeedValue);

            Animator.SetBool(CharacterAnimatorNamingList.MovementPressedParameterName, GetMovementPressed());

            Animator.SetFloat(CharacterAnimatorNamingList.ControllerDeltaParameterName, ReturnDeltaMovement());

            Animator.SetBool(CharacterAnimatorNamingList.UsingControllerParameterName, InputHelper.DeviceInputTool.IsUsingController());

            Animator.SetBool(CharacterAnimatorNamingList.CharacterHasStaminaParameterName, CharacterState.CanUseStamina);

            Animator.SetBool(CharacterAnimatorNamingList.isGroundedParameterName, CharacterState.isGrounded);
            
            Animator.SetFloat(CharacterAnimatorNamingList.NormalizedTimeParameterName, Mathf.Repeat(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        }

        private void HandleJump()
        {
            if(CharacterState.TryingToJump && CharacterState.CanJump)
            {
                Animator.SetTrigger(CharacterAnimatorNamingList.JumpTriggerParameterName);
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
                if(CharacterState.CanUseStamina==false || Animator.GetBool(CharacterAnimatorNamingList.InterruptableParameterName))
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
