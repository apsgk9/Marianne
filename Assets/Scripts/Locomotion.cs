using System;
using UnityEngine;
using UnityEngine.AI;

public class Locomotion : ILocomotion
{
    private readonly Player _player;
    private readonly CharacterController _characterController;
    private float _moveSpeed;
    private float _runMoveSpeed;
    private Camera _playerCamera;

    private float _RotationSpeed { get;}
    public Vector3 VectorForwardBasedOnPlayerCamera { get; private set; }
    private Vector3 _movementInput;
    private RunTransitionHandler _runTransitionHandler;

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

    public Locomotion(Player player, float moveSpeed,float runMoveSpeed,float rotationSpeed,Camera playerCamera,RunTransitionHandler _runTransitionHandlerInput)
    {
        _player = player;
        _characterController = player.GetComponent<CharacterController>();
        _moveSpeed = moveSpeed;
        _runMoveSpeed=runMoveSpeed;
        _playerCamera= playerCamera;
        _RotationSpeed = rotationSpeed;
        if(_playerCamera==null)
        {
            var temp =GameObject.FindObjectOfType<PlayerCamera>();
            _playerCamera = temp?.GetComponent<Camera>();
        }
        _runTransitionHandler = _runTransitionHandlerInput;
        previousAnimatorMovementSpeed=0f;
    }

    public void Tick()
    {
        CalculatePlayerForwardVector();
        MoveTransform();
        RotateTransform();
    }

    private void RotateTransform()
    {
        if (VectorForwardBasedOnPlayerCamera != Vector3.zero)
        {
            var DesiredRotation = Quaternion.LookRotation(VectorForwardBasedOnPlayerCamera);
            var FixedRotation = Quaternion.Slerp(_player.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed);
            
            //zero out y
            FixedRotation= Quaternion.Euler(0f,FixedRotation.eulerAngles.y,0f);
            _player.transform.rotation = FixedRotation;
        }
    }

    private void CalculatePlayerForwardVector()
    {
        //figure how to differentiate console input and keyboard input
        _movementInput= new Vector3(PlayerCharacterInput.Instance.Horizontal, 0, PlayerCharacterInput.Instance.Vertical);

        var temp=_playerCamera.transform.TransformDirection(_movementInput);
        temp.y=0f;
        VectorForwardBasedOnPlayerCamera=temp;
    }

    private void MoveTransform()
    {
        //MovementSystem1();
        //MovementSystem2();
        MovementSystem3();
    }

    private void MovementSystem2()
    {
        var movementMagnitude = _movementInput.magnitude;
        float movementSpeed = 0f;
        float movementAnimatorSpeed = 0f;
        if (movementMagnitude >= runThreshold && movementMagnitude < sprintThreshold) //run
        {
            movementSpeed = runSpeed;
            movementAnimatorSpeed = 2f;
        }
        else if (movementMagnitude >= sprintThreshold) //sprint
        {
            movementSpeed = sprintSpeed;
            movementAnimatorSpeed = 3f;
        }
        else if (movementMagnitude < runThreshold && movementMagnitude > 0.1f) //walk
        {
            movementSpeed = walkSpeed;
            movementAnimatorSpeed = 1f;
        }

        //var runMultiplierTarget = _runTransitionHandler.RunMultiplier;
        //var runModifierAddition = ((runMultiplierTarget - (float)_runTransitionHandler.baseTarget) * _runMoveSpeed);

        Vector3 baseMovementComposite = VectorForwardBasedOnPlayerCamera.normalized * movementSpeed;

        //Vector3 baseMovementComposite = (VectorForwardBasedOnPlayerCamera.normalized * movementMagnitude * _moveSpeed);
        finalMovementComposite = baseMovementComposite;

        var DeltaMovementSpeed = movementAnimatorSpeed - previousAnimatorMovementSpeed;



        _characterController.SimpleMove(finalMovementComposite);
        OnMoveChange?.Invoke(finalMovementComposite);
        OnMoveAnimatorSpeedChange?.Invoke(movementAnimatorSpeed);
        previousAnimatorMovementSpeed = movementAnimatorSpeed;
    }

    private void MovementSystem1()
    {
        var movementMagnitude = _movementInput.magnitude * 2;
        
        float runModifierAddition=0f;

        if(PlayerCharacterInput.Instance.RunPressed)
        {
            runModifierAddition=_runMoveSpeed;
        }
        Vector3 runcomposite = VectorForwardBasedOnPlayerCamera.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (VectorForwardBasedOnPlayerCamera.normalized * movementMagnitude * _moveSpeed);
        finalMovementComposite = runcomposite + baseMovementComposite;


        _characterController.SimpleMove(finalMovementComposite);
        OnMoveChange?.Invoke(finalMovementComposite);
        OnMoveAnimatorSpeedChange?.Invoke(finalMovementComposite.magnitude);
    }

    private void MovementSystem3()
    {
        var movementMagnitude = Mathf.Clamp(_movementInput.magnitude,0,1);
        if(movementMagnitude>1)
        {
            Debug.Log(movementMagnitude.ToString("F20"));
        }

        var runMultiplierTarget = _runTransitionHandler.RunMultiplier;
        var runModifierAddition = ((runMultiplierTarget - (float)_runTransitionHandler.baseTarget) * _runMoveSpeed);

        Vector3 runcomposite = VectorForwardBasedOnPlayerCamera.normalized * runModifierAddition;

        Vector3 baseMovementComposite = (VectorForwardBasedOnPlayerCamera.normalized * movementMagnitude * _moveSpeed);
        finalMovementComposite = runcomposite + baseMovementComposite;
        
        var finalMovementCompositeMagnitude=finalMovementComposite.magnitude;

        if (finalMovementCompositeMagnitude >= runThreshold && finalMovementCompositeMagnitude <= sprintThreshold) //run
        {
            finalMovementComposite =finalMovementComposite.normalized*runSpeed;
        }
        else if (finalMovementCompositeMagnitude > sprintThreshold+ Mathf.Epsilon) //sprint
        {
            Debug.Log(finalMovementCompositeMagnitude.ToString("F64") +"||"+sprintThreshold);
            finalMovementComposite =finalMovementComposite.normalized*sprintSpeed;
        }
        else if (finalMovementCompositeMagnitude < runThreshold && finalMovementCompositeMagnitude > 0.01f) //walk
        {
            finalMovementComposite =finalMovementComposite.normalized*walkSpeed;
        }

        _characterController.SimpleMove(finalMovementComposite);
        OnMoveChange?.Invoke(finalMovementComposite);
        OnMoveAnimatorSpeedChange?.Invoke(finalMovementComposite.magnitude/2);
    }
}
