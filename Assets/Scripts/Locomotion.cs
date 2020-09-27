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
        _movementInput= new Vector3(PlayerCharacterInput.Instance.Horizontal, 0, PlayerCharacterInput.Instance.Vertical);
        var temp=_playerCamera.transform.TransformDirection(_movementInput);
        temp.y=0f;
        VectorForwardBasedOnPlayerCamera=temp;
    }

    private void MoveTransform()
    {
        var movementMagnitude=_movementInput.magnitude;

        var runMultiplierTarget= _runTransitionHandler.RunMultiplier;
        var runModifierAddition= ((runMultiplierTarget-(float)_runTransitionHandler.baseTarget)*_runMoveSpeed);

        Vector3 runcomposite=VectorForwardBasedOnPlayerCamera.normalized*runModifierAddition;

        Vector3 baseMovementComposite=(VectorForwardBasedOnPlayerCamera.normalized*movementMagnitude*_moveSpeed);

        finalMovementComposite=runcomposite+baseMovementComposite;


        _characterController.SimpleMove(finalMovementComposite);
        OnMoveChange?.Invoke(finalMovementComposite);
    }

}
