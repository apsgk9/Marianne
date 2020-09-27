using System;
using UnityEngine;
using UnityEngine.AI;

public class Locomotion : ILocomotion
{
    private readonly Player _player;
    private readonly CharacterController _characterController;
    [SerializeField] private float _moveSpeed;

    private Camera _playerCamera;

    private float _RotationSpeed { get;}
    public Vector3 VectorForwardBasedOnPlayerCamera { get; private set; }
    private Vector3 _movementInput;

    public Locomotion(Player player, float moveSpeed,float rotationSpeed,Camera playerCamera)
    {
        _player = player;
        _characterController = player.GetComponent<CharacterController>();
        _moveSpeed = moveSpeed;
        _playerCamera= playerCamera;
        _RotationSpeed = rotationSpeed;
        if(_playerCamera==null)
        {
            var temp =GameObject.FindObjectOfType<PlayerCamera>();
            _playerCamera = temp?.GetComponent<Camera>();
        }
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
        //fix movement since camera uptop can slow locomotion
        _movementInput= new Vector3(PlayerCharacterInput.Instance.Horizontal, 0, PlayerCharacterInput.Instance.Vertical);
        var temp=_playerCamera.transform.TransformDirection(_movementInput);
        temp.y=0f;
        VectorForwardBasedOnPlayerCamera=temp;
    }

    private void MoveTransform()
    {
        var movementMagnitude=_movementInput.magnitude;
        _characterController.SimpleMove(VectorForwardBasedOnPlayerCamera.normalized*movementMagnitude*_moveSpeed);
    }
}
