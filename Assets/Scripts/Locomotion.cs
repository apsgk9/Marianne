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
        MoveTransform();
        RotateTransform();

    }

    private void RotateTransform()
    {
        var direction = GetPlayerForwardVector();
        if (direction != Vector3.zero)
        {
            var DesiredRotation = Quaternion.LookRotation(direction);
            var FixedRotation = Quaternion.Slerp(_player.transform.rotation, DesiredRotation, Time.deltaTime * _RotationSpeed);
            
            //zero out y
            FixedRotation= Quaternion.Euler(0f,FixedRotation.eulerAngles.y,0f);
            _player.transform.rotation = FixedRotation;
        }
    }

    private Vector3 GetPlayerForwardVector(float multipler=1f)
    {
        var movementInput= new Vector3(PlayerInput.Instance.Horizontal*multipler, 0, PlayerInput.Instance.Vertical*multipler);
        Vector3 VectorForwardBasedOnPlayerCamera = _playerCamera.transform.TransformDirection(movementInput);
        return VectorForwardBasedOnPlayerCamera;
    }

    private void MoveTransform()
    {
        
        _characterController.SimpleMove(GetPlayerForwardVector(_moveSpeed));
    }
}
