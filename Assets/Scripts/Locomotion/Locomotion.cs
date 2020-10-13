using System;
using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

public partial class Locomotion : ILocomotion
{
    private readonly GameObject _characterGameObject;
    private readonly ICharacterMover _characterMover;
    private float _moveSpeed;
    private float _runMoveSpeed;
    private Camera _characterCamera;
    private float _RotationSpeed { get;}
    private RootMotionDelta _RootMotionDelta;
    public Vector3 VectorForwardBasedOnPlayerCamera { get; private set; }
    private Vector3 _movementInput;
    public Vector3 finalMovementComposite{get; private set;}
    public Vector3 DeltaMovement => finalMovementComposite;
    public event Action<Vector3> OnMoveChange;
    public event Action<float> OnMoveAnimatorSpeedChange;
    public float runThreshold=0.5f;
    public float sprintThreshold=2.01f;
    public float sprintSpeed=6f;
    public float runSpeed=4f;
    public float walkSpeed=2f;
    private LocomotionMode locomotionMode;
    public AnimationCurve _MovementVectorBlend;
    public AnimationCurve _RotationBlend;
    [SerializeReference]
    public ICharacterInput _characterInput;

    public Locomotion(GameObject character, float moveSpeed,float runMoveSpeed,float rotationSpeed,
    Camera characterCamera,AnimationCurve movementVectorBlend,AnimationCurve rotationBlend,
    ICharacterInput characterInput,ICharacterMover characterMover)
    {
        _characterGameObject = character;
        _characterMover = characterMover;
        _moveSpeed = moveSpeed;
        _runMoveSpeed=runMoveSpeed;
        _characterCamera= characterCamera;
        _RotationSpeed = rotationSpeed;
        _MovementVectorBlend=movementVectorBlend;
        _RotationBlend=rotationBlend;
        _characterInput=characterInput;


        _RootMotionDelta = _characterGameObject.GetComponentInChildren<RootMotionDelta>();
        locomotionMode= LocomotionMode.Idle;

        _RootMotionDelta.OnRootMotionChange+=HandleRootMotion;
    }
    private void OnDestroy()
    {
        _RootMotionDelta.OnRootMotionChange-=HandleRootMotion;
    }

    private void HandleRootMotion(Vector3 DeltaVector, Quaternion NewRotation)
    {
        float angleDifference = Vector3.Angle(DeltaVector,VectorForwardBasedOnPlayerCamera.normalized);
        var multiplier=0f;        
        multiplier=_MovementVectorBlend.Evaluate((180f-angleDifference)/180f);
        var baseMovementComposite= DeltaVector* (multiplier);
        
        _characterMover.Move(baseMovementComposite);

        OnMoveChange?.Invoke(DeltaVector);
    }

    public void Tick()
    {
        CalculateCharacterForwardVector();
        RotateTransform();
        SendAnimatorLocomotionCommands(_characterInput.IsRunning());
    }

    private void RotateTransform()
    {
        if (VectorForwardBasedOnPlayerCamera != Vector3.zero)
        {    
            var DesiredRotation = Quaternion.LookRotation(VectorForwardBasedOnPlayerCamera);
            float angleDifference = Vector3.Angle(_characterGameObject.transform.forward,VectorForwardBasedOnPlayerCamera.normalized);
            var multiplier=0f;        
            multiplier=_RotationBlend.Evaluate(angleDifference/180f);

            var FixedRotation = Quaternion.Slerp(_characterGameObject.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed*multiplier);
            
            _characterGameObject.transform.rotation = FixedRotation;
        }
    }

    private void CalculateCharacterForwardVector()
    {
        _movementInput= new Vector3(_characterInput.MovementHorizontal(), 0, _characterInput.MovementVertical());
        VectorForwardBasedOnPlayerCamera = Quaternion.Euler(0,_characterCamera.transform.eulerAngles.y,0)*_movementInput;
    }


    private void SendAnimatorLocomotionCommands(bool isRunning)
    {
        var movementMagnitude = Mathf.Clamp(_movementInput.magnitude,0,1);

        int runModifierAddition = isRunning? 2:0;

        Vector3 runcomposite = VectorForwardBasedOnPlayerCamera.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (VectorForwardBasedOnPlayerCamera.normalized * movementMagnitude * _moveSpeed);
        finalMovementComposite = runcomposite + baseMovementComposite;
        var runGap=0.00f;
        var walkGap=0.00f;
        if(locomotionMode==LocomotionMode.Run)
        {
            walkGap=-0.1f;
            runGap=-0.1f;    
        }
        else if(locomotionMode==LocomotionMode.Walk)
        {       
            walkGap=0.1f;
            runGap=-0.1f;             
        }
        
        var finalMovementCompositeMagnitude=finalMovementComposite.magnitude;

        if (finalMovementCompositeMagnitude >= runThreshold+runGap && finalMovementCompositeMagnitude <= sprintThreshold) //run
        {
            locomotionMode = LocomotionMode.Run;
        }
        else if (finalMovementCompositeMagnitude > sprintThreshold+ Mathf.Epsilon) //sprint
        {
            locomotionMode = LocomotionMode.Sprint;
        }
        else if (finalMovementCompositeMagnitude < runThreshold+walkGap && finalMovementCompositeMagnitude > 0.01f) //walk
        {         
            locomotionMode = LocomotionMode.Walk;
        }
        else
        {
            locomotionMode= LocomotionMode.Idle;
        }
        OnMoveAnimatorSpeedChange?.Invoke((float)locomotionMode);
        
    }

    
    
}
