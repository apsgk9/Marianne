using System;
using CharacterInput;
using CharacterProperties;
using Movement;
using UnityEngine;
using static LocomotionEnmus;

public partial class Locomotion : ILocomotion
{
    private readonly GameObject _characterGameObject;
    private readonly IMover _characterMover;
    private Transform _viewTransform;
    private float _RotationSpeed { get; }
    private RootMotionDelta _RootMotionDelta;

    public Vector3 DesiredCharacterVectorForward { get; private set; }
    private Vector3 _movementInput;
    public Vector3 finalMovementComposite { get; private set; }
    public Quaternion CompositeRotation { get; private set; }
    public bool UseMovementAngleDifference { get; set; }

    public event Action<Vector3> OnMoveChange;
    public event Action<float> OnMoveAnimatorSpeedChange;
    public event Action<bool> OnTryingToJump;
    public event Action<State> OnStateChange;
    public event Action<Vector3> OnLand;
    public event Action<bool> OnCanJump;

    public float runThreshold = 0.5f;
    public float sprintThreshold = 2.01f;
    private LocomotionMode _locomotionMode;
    public LocomotionMode LocomotionMode { get { return _locomotionMode; } }

    public AnimationCurve _MovementVectorBlend;
    public AnimationCurve _RotationBlend;
    [SerializeReference]
    public ICharacterInput _characterInput;

    //--------------------------
    private Vector3 _previousVelocity;
    private State _state= State.Falling;


    //Current momentum;
    protected Vector3 momentum = Vector3.zero;
    //Saved velocity from last frame;
    Vector3 savedVelocity = Vector3.zero;
    //Saved horizontal movement velocity from last frame;
    Vector3 savedMovementVelocity = Vector3.zero;
    private float slopeLimit = 60f;

    [Tooltip("Whether to calculate and apply momentum relative to the controller's transform.")]
    public bool useLocalMomentum = false;
    private int movementSpeed;
    //'Aircontrol' determines to what degree the player is able to move while in the air;
    [Range(0f, 1f)]
    public float airControl = 0.4f;
    private float jumpDuration = 0.02f;
    private float currentJumpStartTime = 0f;

    private Vector3 JumpVelocity;

    private bool _canJump;
    private float gravity=Physics.gravity.y;
    private float slideGravity=5f;
	public float airFriction = 0.5f;
	public float groundFriction = 100f;

    public Locomotion(GameObject character, float rotationSpeed,
    Transform viewTransform, AnimationCurve movementVectorBlend, AnimationCurve rotationBlend,
    ICharacterInput characterInput, IMover characterMover)
    {
        _characterGameObject = character;
        _characterMover = characterMover;
        _viewTransform = viewTransform;
        _RotationSpeed = rotationSpeed;
        _MovementVectorBlend = movementVectorBlend;
        _RotationBlend = rotationBlend;
        _characterInput = characterInput;


        _RootMotionDelta = _characterGameObject.GetComponentInChildren<RootMotionDelta>();
        _locomotionMode = LocomotionMode.Idle;
        if (_RootMotionDelta != null)
        {
            _RootMotionDelta.OnRootMotionChange += HandleRootMotion;
        }
        UseMovementAngleDifference = true;
        
        OnStateChange?.Invoke(_state);
    }
    private void OnDestroy()
    {
        if (_RootMotionDelta != null)
        {
            _RootMotionDelta.OnRootMotionChange -= HandleRootMotion;
        }
    }

    private void HandleRootMotion(Vector3 DeltaVector, Quaternion NewRotation)
    {
        
        if (GameManager.Instance.isPaused || Time.deltaTime == 0f) //there is a bug that sets Time.deltaTime infinity to, this fixes it
            return;


        Vector3 _velocity = Vector3.zero;

        ////Calculate the final velocity for this frame;
        //[...]
        float angleDifference = Vector3.Angle(DeltaVector, DesiredCharacterVectorForward.normalized);
        var multiplier = 1f;

        if (_characterMover.IsGrounded() && UseMovementAngleDifference)
        {
            multiplier = _MovementVectorBlend.Evaluate((180f - angleDifference) / 180f);
        }

        //Apply friction and gravity to 'momentum';
		HandleMomentum();

        //If local momentum is used, transform momentum into world space first;
		Vector3 _worldMomentum = momentum;
		if(useLocalMomentum)
			_worldMomentum = _characterGameObject.transform.localToWorldMatrix * momentum;

		//Add current momentum to velocity;
		_velocity += _worldMomentum;

        ////If the character is grounded, extend ground detection sensor range;
        _characterMover.SetExtendSensorRange(IsGrounded());

        ////Set mover velocity;
        //mover.SetVelocity(_velocity);
        var Movement = (DeltaVector / Time.deltaTime);

        //if (IsGrounded())
        //{
        //    Movement*= multiplier;
        //}
        //if(_state==State.Falling)
        //{
        //    Movement = GetFallingVelocity();
        //}
        Movement+=_velocity;
        
        _characterMover.SetVelocity(Movement);            
        _previousVelocity=Movement;
        OnMoveChange?.Invoke(DeltaVector);    

        //Store velocity for next frame;
		savedVelocity = _velocity;
		savedMovementVelocity = _velocity - _worldMomentum;
        
    }

    public void Tick()
    {

        ////Run initial mover ground check;
        //mover.CheckForGround();
        _characterMover.CheckForGround();

        _state= DetermineControllerState();
        OnStateChange?.Invoke(_state);
        //Debug.Log("STATE:"+_state.ToString());

		//Apply friction and gravity to 'momentum';
		////HandleMomentum();

        var Jumping = HandleJump();

        Vector3 _velocity = Vector3.zero;


		//If local momentum is used, transform momentum into world space first;
		////Vector3 _worldMomentum = momentum;
		////if(useLocalMomentum)
		////	_worldMomentum = _characterGameObject.transform.localToWorldMatrix * momentum;

		////Add current momentum to velocity;
		////_velocity += _worldMomentum;
		
		//If player is grounded or sliding on a slope, extend mover's sensor range;
		//This enables the player to walk up/down stairs and slopes without losing ground contact;
		_characterMover.SetExtendSensorRange(IsGrounded());

        //Set Velocity
        if (IsGrounded())
        {
            _characterMover.SetVelocity(_previousVelocity + _characterMover.GetCurrentGroundAdjustmentVelocity());
            if (_RootMotionDelta.canRotate)
            {
                CalculateCharacterDesiredVector();
                RotateTransform(DesiredCharacterVectorForward);
            }
            SendAnimatorLocomotionCommands(_characterInput.IsRunning());
            
        }
        else
        {
            //Vector3 fallingvelocity = GetFallingVelocity();
            //_characterMover.SetVelocity(fallingvelocity);
            //_previousVelocity = fallingvelocity;
        }
        //Store velocity for next frame;
		////savedVelocity = _velocity;
		////savedMovementVelocity = _velocity - _worldMomentum;
    }
    

    private Vector3 GetFallingVelocity()
    {
        return _previousVelocity + Physics.gravity * Time.deltaTime;
    }

    private bool HandleJump()
    {
        if (!_characterMover.IsGrounded())
        {
            OnTryingToJump?.Invoke(false);
            return false;
        }

        bool TryingToJump = _characterInput.AttemptingToJump();
        OnTryingToJump?.Invoke(TryingToJump);
        CalculateCanJump(TryingToJump);

        

        return TryingToJump;
    }

    private void CalculateCanJump(bool TryingToJump)
    {
        

        if (TryingToJump == false) //released/up
        {
            if (_canJump == false)
            {                
                _canJump = true;
                OnCanJump?.Invoke(true);
            }
        }
        else
        {
            if (_canJump == true)
            {                
                OnGroundContactLost();
                _state=State.Jumping;
                OnStateChange?.Invoke(_state);
                currentJumpStartTime = Time.time;
            }

        }
    }

    public void ApplyRotation(Quaternion FinalRotation)
    {
        _characterGameObject.transform.rotation = FinalRotation;
    }

    private void RotateTransform(Vector3 ForwardDirection)
    {
        if (ForwardDirection != Vector3.zero)
        {
            var DesiredRotation = Quaternion.LookRotation(ForwardDirection);
            float angleDifference = Vector3.Angle(_characterGameObject.transform.forward, ForwardDirection.normalized);
            var multiplier = 0f;
            multiplier = _RotationBlend.Evaluate(angleDifference / 180f);

            CompositeRotation = Quaternion.Slerp(_characterGameObject.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed * multiplier);

            ApplyRotation(CompositeRotation);
        }
    }

    private void CalculateCharacterDesiredVector()
    {
        _movementInput = new Vector3(_characterInput.MovementHorizontal(), 0, _characterInput.MovementVertical());
        DesiredCharacterVectorForward = Quaternion.Euler(0, _viewTransform.eulerAngles.y, 0) * _movementInput;
    }

    private void SendAnimatorLocomotionCommands(bool isRunning)
    {
        var movementMagnitude = Mathf.Clamp(_movementInput.magnitude, 0, 1);

        int runModifierAddition = isRunning ? 2 : 0;

        Vector3 runcomposite = DesiredCharacterVectorForward.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (DesiredCharacterVectorForward.normalized * movementMagnitude);
        finalMovementComposite = runcomposite + baseMovementComposite;
        var runGap = 0.00f;
        var walkGap = 0.00f;
        if (_locomotionMode == LocomotionMode.Run)
        {
            walkGap = -0.1f;
            runGap = -0.1f;
        }
        else if (_locomotionMode == LocomotionMode.Walk)
        {
            walkGap = 0.1f;
            runGap = -0.1f;
        }

        var finalMovementCompositeMagnitude = finalMovementComposite.magnitude;

        if (finalMovementCompositeMagnitude >= runThreshold + runGap && finalMovementCompositeMagnitude <= sprintThreshold) //run
        {
            _locomotionMode = LocomotionMode.Run;
        }
        else if (finalMovementCompositeMagnitude > sprintThreshold + Mathf.Epsilon) //sprint
        {
            _locomotionMode = LocomotionMode.Sprint;
        }
        else if (finalMovementCompositeMagnitude < runThreshold + walkGap && finalMovementCompositeMagnitude > 0.01f) //walk
        {
            _locomotionMode = LocomotionMode.Walk;
        }
        else
        {
            _locomotionMode = LocomotionMode.Idle;
        }
        OnMoveAnimatorSpeedChange?.Invoke((float)_locomotionMode);
        movementSpeed = (int)_locomotionMode;

    }

    public void Jump(float Height)
    { 
        float initialVelocity = (float)Math.Sqrt((-2*Physics.gravity.y)); //(Height+(0.5f*Physics.gravity.y*jumpDuration*jumpDuration))/jumpDuration;
        Vector3 UpwardVelocity = _characterGameObject.transform.up * initialVelocity;
        Vector3 GroundVelocity = _previousVelocity;
        GroundVelocity.y = 0f;

        Vector3 FowardVelocity = GroundVelocity;
        JumpVelocity = UpwardVelocity + FowardVelocity;        

        _canJump = false;
        OnCanJump?.Invoke(false);
        
         
    }








    private State DetermineControllerState()
    {
        //Check if vertical momentum is pointing upwards;
        bool _isRising = IsRisingOrFalling() && (VectorMath.GetDotProduct(GetMomentum(), _characterGameObject.transform.up) > 0f);
        //Check if controller is sliding;
        bool _isSliding = _characterMover.IsGrounded() && IsGroundTooSteep();

        //Debug.Log("_isRising: "+_isRising);
        //Debug.Log("sRisingOrFalling(): "+IsRisingOrFalling());
        //Debug.Log("ENTER State: "+_state.ToString());

        //Grounded;
        if (_state == State.Grounded)
        {
            if (_isRising)
            {
                //Debug.Log("Rising");
                OnGroundContactLost();
                return State.Rising;
            }
            if (!_characterMover.IsGrounded())
            {
                //Debug.Log("Falling");
                OnGroundContactLost();
                return State.Falling;
            }
            if (_isSliding)
            {
                //Debug.Log("Sliding");
                return State.Sliding;
            }
            //Debug.Log("Grounded");
            return State.Grounded;
        }

        //Falling;
        if (_state == State.Falling)
        {
            if (_isRising)
            {
                //Debug.Log("Rising");
                return State.Rising;
            }
            if (_characterMover.IsGrounded() && !_isSliding)
            {
                //Debug.Log("Grounded");
                OnGroundContactRegained(momentum);
                return State.Grounded;
            }
            if (_isSliding)
            {
                //Debug.Log("Sliding");
                OnGroundContactRegained(momentum);
                return State.Sliding;
            }

            //Debug.Log("Falling");
            return State.Falling;
        }

        //Sliding;
        if (_state == State.Sliding)
        {
            if (_isRising)
            {
                OnGroundContactLost();
                return State.Rising;
            }
            if (!_characterMover.IsGrounded())
            {
                return State.Falling;
            }
            if (_characterMover.IsGrounded() && !_isSliding)
            {
                OnGroundContactRegained(momentum);
                return State.Grounded;
            }
            return State.Sliding;
        }

        //Rising;
        if (_state == State.Rising)
        {
            if (!_isRising)
            {
                if (_characterMover.IsGrounded() && !_isSliding)
                {
                    OnGroundContactRegained(momentum);
                    return State.Grounded;
                }
                if (_isSliding)
                {
                    return State.Sliding;
                }
                if (!_characterMover.IsGrounded())
                {
                    return State.Falling;
                }
            }

            ////If a ceiling detector has been attached to this gameobject, check for ceiling hits;
            //if(ceilingDetector != null)
            //{
            //	if(ceilingDetector.HitCeiling())
            //	{
            //		OnCeilingContact();
            //		return State.Falling;
            //	}
            //}
            return State.Rising;
        }

        //Jumping;
        if (_state == State.Jumping)
        {
            //Check for jump timeout;
            if ((Time.time - currentJumpStartTime) > jumpDuration)
                return State.Rising;

            //Check if jump key was let go;
            //if(jumpKeyWasLetGo)
            //	return State.Rising;
            //if (UserInput.Instance.JumpPressed == false)
            //{
            //    return State.Rising;
            //}
                

            ////If a ceiling detector has been attached to this gameobject, check for ceiling hits;
            //if(ceilingDetector != null)
            //{
            //	if(ceilingDetector.HitCeiling())
            //	{
            //		OnCeilingContact();
            //		return State.Falling;
            //	}
            //}
            return State.Jumping;
        }
        return State.Falling;
    }

    public Vector3 GetMomentum()
    {
        Vector3 _worldMomentum = momentum;
        if (useLocalMomentum)
            _worldMomentum = _characterGameObject.transform.localToWorldMatrix * momentum;

        return _worldMomentum;
    }

    //Apply friction to both vertical and horizontal momentum based on 'friction' and 'gravity';
    //Handle sliding down steep slopes;
    void HandleMomentum()
    {
        //If local momentum is used, transform momentum into world coordinates first;
        if (useLocalMomentum)
            momentum = _characterGameObject.transform.localToWorldMatrix * momentum;

        Vector3 _verticalMomentum = Vector3.zero;
        Vector3 _horizontalMomentum = Vector3.zero;

        //Split momentum into vertical and horizontal components;
        if (momentum != Vector3.zero)
        {
            _verticalMomentum = VectorMath.ExtractDotVector(momentum, _characterGameObject.transform.up);
            _horizontalMomentum = momentum - _verticalMomentum;
        }

        //Add gravity to vertical momentum;
        _verticalMomentum += _characterGameObject.transform.up * gravity * Time.deltaTime;

        //Remove any downward force if the controller is grounded;
        if (_state == State.Grounded)
            _verticalMomentum = Vector3.zero;

        //Apply friction to horizontal momentum based on whether the controller is grounded;
        if (_state == State.Grounded)
            _horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(_horizontalMomentum, groundFriction, Time.deltaTime, Vector3.zero);
        else
            _horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(_horizontalMomentum, airFriction, Time.deltaTime, Vector3.zero);

        //Add horizontal and vertical momentum back together;
        momentum = _horizontalMomentum + _verticalMomentum;
        

        //Project the current momentum onto the current ground normal if the controller is sliding down a slope;
        if (_state == State.Sliding)
        {
            momentum = Vector3.ProjectOnPlane(momentum, _characterMover.GetGroundNormal());
        }

        //Apply slide gravity along ground normal, if controller is sliding;
        if (_state == State.Sliding)
        {
            Vector3 _slideDirection = Vector3.ProjectOnPlane(-_characterGameObject.transform.up, _characterMover.GetGroundNormal()).normalized;
            momentum += _slideDirection * slideGravity * Time.deltaTime;
        }

        //If controller is jumping, override vertical velocity with jumpSpeed;
        if (_state == State.Jumping)
        {
            //momentum = VectorMath.RemoveDotVector(momentum, _characterGameObject.transform.up);
            //momentum += _characterGameObject.transform.up * jumpSpeed;
            if(JumpVelocity!=Vector3.zero)
            {
                momentum.x=0f;
                momentum.z=0f;
                momentum += JumpVelocity;
                JumpVelocity=Vector3.zero;
            }
            
        }

        if (useLocalMomentum)
            momentum = _characterGameObject.transform.worldToLocalMatrix * momentum;
        
    }

    //This function is called when the controller has landed on a surface after being in the air;
    void OnGroundContactRegained(Vector3 _collisionVelocity)
    {
        //Call 'OnLand' event;
        if (OnLand != null)
            OnLand(_collisionVelocity);
    }

    void OnGroundContactLost()
    {
        //Calculate current velocity;
        //If velocity would exceed the controller's movement speed, decrease movement velocity appropriately;
        //This prevents unwanted accumulation of velocity;
        float _horizontalMomentumSpeed = VectorMath.RemoveDotVector(GetMomentum(), _characterGameObject.transform.up).magnitude;
        Vector3 _currentVelocity = GetMomentum() + Vector3.ClampMagnitude(savedMovementVelocity, Mathf.Clamp(movementSpeed - _horizontalMomentumSpeed, 0f, movementSpeed));

        //Calculate length and direction from '_currentVelocity';
        float _length = _currentVelocity.magnitude;

        //Calculate velocity direction;
        Vector3 _velocityDirection = Vector3.zero;
        if (_length != 0f)
            _velocityDirection = _currentVelocity / _length;

        //Subtract from '_length', based on 'movementSpeed' and 'airControl', check for overshooting;
        if (_length >= movementSpeed * airControl)
            _length -= movementSpeed * airControl;
        else
            _length = 0f;

        //If local momentum is used, transform momentum into world coordinates first;
        if (useLocalMomentum)
            momentum = _characterGameObject.transform.localToWorldMatrix * momentum;

        momentum = _velocityDirection * _length;

        if (useLocalMomentum)
            momentum = _characterGameObject.transform.worldToLocalMatrix * momentum;
    }

    //Helper functions;

    //Returns 'true' if vertical momentum is above a small threshold;
    private bool IsRisingOrFalling()
    {
        //Calculate current vertical momentum;
        Vector3 _verticalMomentum = VectorMath.ExtractDotVector(GetMomentum(), _characterGameObject.transform.up);

        //Setup threshold to check against;
        //For most applications, a value of '0.001f' is recommended;
        float _limit = 0.001f;

        //Return true if vertical momentum is above '_limit';
        return (_verticalMomentum.magnitude > _limit);
    }

    //Returns true if angle between controller and ground normal is too big (> slope limit), i.e. ground is too steep;
    private bool IsGroundTooSteep()
    {
        if (!_characterMover.IsGrounded())
            return true;

        return (Vector3.Angle(_characterMover.GetGroundNormal(), _characterGameObject.transform.up) > slopeLimit);
    }

    //Returns 'true' if controller is grounded (or sliding down a slope);
	public bool IsGrounded()
	{
		return(_state == State.Grounded || _state == State.Sliding);
	}


}
