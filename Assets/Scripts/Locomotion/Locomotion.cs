using System;
using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

public partial class Locomotion : ILocomotion
{
    private readonly GameObject _characterGameObject;
    private readonly ICharacterMover _characterMover;
    private Transform _viewTransform;
    private float _RotationSpeed { get;}
    private RootMotionDelta _RootMotionDelta;
    public Vector3 DesiredCharacterVectorForward { get; private set; }
    private Vector3 _movementInput;
    public Vector3 finalMovementComposite{get; private set;}
    public event Action<Vector3> OnMoveChange;
    public event Action<float> OnMoveAnimatorSpeedChange;
    public float runThreshold=0.5f;
    public float sprintThreshold=2.01f;
    private LocomotionMode locomotionMode;
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
        locomotionMode= LocomotionMode.Idle;
        if(_RootMotionDelta!=null)
        {
            _RootMotionDelta.OnRootMotionChange+=HandleRootMotion;
        }
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
        float angleDifference = Vector3.Angle(DeltaVector,DesiredCharacterVectorForward.normalized);
        var multiplier=0f;        
        multiplier=_MovementVectorBlend.Evaluate((180f-angleDifference)/180f);
        var baseMovementComposite= DeltaVector* (multiplier);
        
        _characterMover.Move(baseMovementComposite);

        OnMoveChange?.Invoke(DeltaVector);
    }

    public void Tick()
    {
        CalculateCharacterDesiredVector();
        RotateTransform();
        SendAnimatorLocomotionCommands(_characterInput.IsRunning());
    }

    private void RotateTransform()
    {
        if (DesiredCharacterVectorForward != Vector3.zero)
        {    
            var DesiredRotation = Quaternion.LookRotation(DesiredCharacterVectorForward);
            float angleDifference = Vector3.Angle(_characterGameObject.transform.forward,DesiredCharacterVectorForward.normalized);
            var multiplier=0f;        
            multiplier=_RotationBlend.Evaluate(angleDifference/180f);

            var FixedRotation = Quaternion.Slerp(_characterGameObject.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed*multiplier);
            
            _characterGameObject.transform.rotation = FixedRotation;
        }
    }

    private void CalculateCharacterDesiredVector()
    {
        _movementInput= new Vector3(_characterInput.MovementHorizontal(), 0, _characterInput.MovementVertical());
        ClearConsole.clear();
        Debug.Log(_movementInput);
        DesiredCharacterVectorForward = Quaternion.Euler(0,_viewTransform.eulerAngles.y,0)*_movementInput;
    }


    private void SendAnimatorLocomotionCommands(bool isRunning)
    {
        var movementMagnitude = Mathf.Clamp(_movementInput.magnitude,0,1);
        ClearConsole.clear();
        Debug.Log(movementMagnitude);

        int runModifierAddition = isRunning? 2:0;

        Vector3 runcomposite = DesiredCharacterVectorForward.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (DesiredCharacterVectorForward.normalized * movementMagnitude);
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