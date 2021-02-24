using System;
using CharacterInput;
using CharacterProperties;
using UnityEngine;
using UnityEngine.AI;
using static LocomotionEnmus;

public partial class Locomotion : ILocomotion
{
    private readonly GameObject _characterGameObject;
    private readonly ICharacterMover _characterMover;
    private Transform _viewTransform;
    private float _RotationSpeed { get;}
    private RootMotionDelta _RootMotionDelta;
    private ICheckGrounded _CheckGrounded;

    public Vector3 DesiredCharacterVectorForward { get; private set; }
    private Vector3 _movementInput;
    public Vector3 finalMovementComposite{get; private set;}
    public Quaternion CompositeRotation { get; private set; }
    public bool UseMovementAngleDifference { get; set; }


    public event Action<Vector3> OnMoveChange;
    public event Action<float> OnMoveAnimatorSpeedChange;
    public event Action<bool> OnJump;

    public float runThreshold=0.5f;
    public float sprintThreshold=2.01f;
    private LocomotionMode _locomotionMode;
    public LocomotionMode LocomotionMode{get {return _locomotionMode;}}
    public AnimationCurve _MovementVectorBlend;
    public AnimationCurve _RotationBlend;
    [SerializeReference]
    public ICharacterInput _characterInput;
    public Locomotion(GameObject character,float rotationSpeed,
    Transform viewTransform,AnimationCurve movementVectorBlend,AnimationCurve rotationBlend,
    ICharacterInput characterInput,ICharacterMover characterMover)
    {
        _characterGameObject = character;
        _characterMover = characterMover;
        _viewTransform= viewTransform;
        _RotationSpeed = rotationSpeed;
        _MovementVectorBlend=movementVectorBlend;
        _RotationBlend=rotationBlend;
        _characterInput=characterInput;


        _RootMotionDelta = _characterGameObject.GetComponentInChildren<RootMotionDelta>();
        _CheckGrounded = _characterGameObject.GetComponentInChildren<ICheckGrounded>();
        _locomotionMode= LocomotionMode.Idle;
        if(_RootMotionDelta!=null)
        {
            _RootMotionDelta.OnRootMotionChange+=HandleRootMotion;
        }
        UseMovementAngleDifference=true;
    }
    private void OnDestroy()
    {
        if(_RootMotionDelta!=null)
        {
            _RootMotionDelta.OnRootMotionChange-=HandleRootMotion;
        }
    }

    private void HandleRootMotion(Vector3 DeltaVector, Quaternion NewRotation)
    {
        if(GameManager.Instance.isPaused ||Time.deltaTime==0f) //there is a bug that sets infinity to 
            return;
        float angleDifference = Vector3.Angle(DeltaVector,DesiredCharacterVectorForward.normalized);
        var multiplier=1f;
        if(UseMovementAngleDifference)
        {
            multiplier=_MovementVectorBlend.Evaluate((180f-angleDifference)/180f);
        }
        if(_CheckGrounded.isGrounded && !_characterInput.AttemptingToJump())
        {
            Debug.Log(_locomotionMode);    

            var onGroundMovement= (DeltaVector/Time.deltaTime)*multiplier;
            _characterMover.SetGroundVelocity(onGroundMovement.x,onGroundMovement.z); 
        }

        //else
        //{
        //    Debug.Log("OnUpdateAnimatorMove");
        //    var baseMovementComposite= DeltaVector* (multiplier);        
        //    _characterMover.OnUpdateAnimatorMove(baseMovementComposite);
        //}
        //var baseMovementComposite= DeltaVector* (multiplier);
        //_characterMover.OnUpdateAnimatorMove(baseMovementComposite);
        
        OnMoveChange?.Invoke(DeltaVector);
    }

    public void Tick()
    {
        var Jumping = HandleJump();
        if(!Jumping &&_CheckGrounded.isGrounded)
        {
            if(_RootMotionDelta.canRotate)
            {
                CalculateCharacterDesiredVector(); //TO DO check if I should put inside
                RotateTransform(DesiredCharacterVectorForward);
            }
            SendAnimatorLocomotionCommands(_characterInput.IsRunning());
        }
        
    }

    private bool HandleJump()
    {
        bool TryingToJump=_characterInput.AttemptingToJump();
        OnJump?.Invoke(TryingToJump);
        return TryingToJump;
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
            float angleDifference = Vector3.Angle(_characterGameObject.transform.forward,ForwardDirection.normalized);
            var multiplier=0f;        
            multiplier=_RotationBlend.Evaluate(angleDifference/180f);

            CompositeRotation = Quaternion.Slerp(_characterGameObject.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed*multiplier);
            
            ApplyRotation(CompositeRotation);
        }
    }

    private void CalculateCharacterDesiredVector()
    {
        _movementInput= new Vector3(_characterInput.MovementHorizontal(), 0, _characterInput.MovementVertical());
        DesiredCharacterVectorForward = Quaternion.Euler(0,_viewTransform.eulerAngles.y,0)*_movementInput;
    }


    private void SendAnimatorLocomotionCommands(bool isRunning)
    {
        var movementMagnitude = Mathf.Clamp(_movementInput.magnitude,0,1);

        int runModifierAddition = isRunning? 2:0;

        Vector3 runcomposite = DesiredCharacterVectorForward.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (DesiredCharacterVectorForward.normalized * movementMagnitude);
        finalMovementComposite = runcomposite + baseMovementComposite;
        var runGap=0.00f;
        var walkGap=0.00f;
        if(_locomotionMode==LocomotionMode.Run)
        {
            walkGap=-0.1f;
            runGap=-0.1f;    
        }
        else if(_locomotionMode==LocomotionMode.Walk)
        {       
            walkGap=0.1f;
            runGap=-0.1f;             
        }
        
        var finalMovementCompositeMagnitude=finalMovementComposite.magnitude;

        if (finalMovementCompositeMagnitude >= runThreshold+runGap && finalMovementCompositeMagnitude <= sprintThreshold) //run
        {
            _locomotionMode = LocomotionMode.Run;
        }
        else if (finalMovementCompositeMagnitude > sprintThreshold+ Mathf.Epsilon) //sprint
        {
            _locomotionMode = LocomotionMode.Sprint;
        }
        else if (finalMovementCompositeMagnitude < runThreshold+walkGap && finalMovementCompositeMagnitude > 0.01f) //walk
        {         
            _locomotionMode = LocomotionMode.Walk;
        }
        else
        {
            _locomotionMode= LocomotionMode.Idle;
        }
        OnMoveAnimatorSpeedChange?.Invoke((float)_locomotionMode);
        
    }

    
    
}
