using System;
using UnityEngine;
using UnityEngine.AI;

public partial class Locomotion : ILocomotion
{
    private readonly Player _player;
    private readonly CharacterController _characterController;
    private float _moveSpeed;
    private float _runMoveSpeed;
    private Camera _playerCamera;

    private float _RotationSpeed { get;}

    private RootMotionDelta _RootMotionDelta;
    public Vector3 VectorForwardBasedOnPlayerCamera { get; private set; }
    private Vector3 _movementInput;
    public Vector3 finalMovementComposite{get; private set;}
    public Vector3 DeltaMovement => finalMovementComposite;
    public event Action<Vector3> OnMoveChange;
    public event Action<float> OnMoveAnimatorSpeedChange;
    public float runThreshold=0.6f;
    public float sprintThreshold=2.01f;
    public float sprintSpeed=6f;
    public float runSpeed=4f;
    public float walkSpeed=2f;
    private float previousAnimatorMovementSpeed;
    private LocomotionMode locomotionMode;
    private event Action<Vector3> Change;
    public AnimationCurve _MovementVectorBlend;
    public AnimationCurve _RotationBlend;

    public Locomotion(Player player, float moveSpeed,float runMoveSpeed,float rotationSpeed,
    Camera playerCamera,AnimationCurve movementVectorBlend,AnimationCurve rotationBlend)
    {
        _player = player;
        _characterController = player.GetComponent<CharacterController>();
        _moveSpeed = moveSpeed;
        _runMoveSpeed=runMoveSpeed;
        _playerCamera= playerCamera;
        _RotationSpeed = rotationSpeed;
        _MovementVectorBlend=movementVectorBlend;
        _RotationBlend=rotationBlend;


        _RootMotionDelta = player.GetComponentInChildren<RootMotionDelta>();
        if(_playerCamera==null)
        {
            var temp =GameObject.FindObjectOfType<PlayerCamera>();
            _playerCamera = temp?.GetComponent<Camera>();
        }
        previousAnimatorMovementSpeed=0f;
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
        _characterController.Move(baseMovementComposite);

        OnMoveChange?.Invoke(DeltaVector);
    }

    public void Tick()
    {
        CalculatePlayerForwardVector();
        RotateTransform();
        SendAnimatorLocomotionCommands(UserInput.Instance.RunPressed);
    }

    private void RotateTransform()
    {
        if (VectorForwardBasedOnPlayerCamera != Vector3.zero)
        {    
            var DesiredRotation = Quaternion.LookRotation(VectorForwardBasedOnPlayerCamera);
            float angleDifference = Vector3.Angle(_player.transform.forward,VectorForwardBasedOnPlayerCamera.normalized);
            var multiplier=0f;        
            multiplier=_RotationBlend.Evaluate(angleDifference/180f);

            var FixedRotation = Quaternion.Slerp(_player.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed*multiplier);
            
            _player.transform.rotation = FixedRotation;
        }
    }

    private void CalculatePlayerForwardVector()
    {
        _movementInput= new Vector3(UserInput.Instance.Horizontal, 0, UserInput.Instance.Vertical);
        VectorForwardBasedOnPlayerCamera = Quaternion.Euler(0,_playerCamera.transform.eulerAngles.y,0)*_movementInput;
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
