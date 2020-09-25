using UnityEngine;
using UnityEngine.AI;

public class Mover : IMover
{
    private readonly Player _player;
    private readonly CharacterController _characterController;
    [SerializeField] private float _moveSpeed;

    private Camera _playerCamera;
    public Mover(Player player, float moveSpeed,Camera playerCamera)
    {
        _player = player;
        _characterController = player.GetComponent<CharacterController>();
        _moveSpeed = moveSpeed;
        _playerCamera= playerCamera;
        if(_playerCamera==null)
        {
            var temp =GameObject.FindObjectOfType<PlayerCamera>();
            _playerCamera = temp?.GetComponent<Camera>();
        }
    }

    public void Tick()
    {
        Vector3 movementInput = new Vector3(PlayerInput.Instance.Horizontal * _moveSpeed, 0, PlayerInput.Instance.Vertical * _moveSpeed);
        Vector3 movement = _playerCamera.transform.TransformDirection(movementInput);
        //Vector3 movement = _player.transform.rotation * movementInput;
        _characterController.SimpleMove(movement);
    }
}
