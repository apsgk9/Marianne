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
    public Vector3 LastDirection { get; private set; }

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
        if(FollowRecenter.Recentering)
        {
            return;            
        }


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
        var movementInput= new Vector3(PlayerCharacterInput.Instance.Horizontal*multipler, 0, PlayerCharacterInput.Instance.Vertical*multipler);
        Vector3 VectorForwardBasedOnPlayerCamera = _playerCamera.transform.TransformDirection(movementInput);
        return VectorForwardBasedOnPlayerCamera;
    }

    private void MoveTransform()
    {
        if(!FollowRecenter.Recentering)
        {
            _characterController.SimpleMove(GetPlayerForwardVector(_moveSpeed));
        }
        else if (PlayerCharacterInput.Instance.IsThereMovement() && !PlayerCharacterInput.Instance.isPlayerTryingToMove)
        {
            _characterController.SimpleMove(_player.transform.forward*_moveSpeed);            
        }
    }
}
