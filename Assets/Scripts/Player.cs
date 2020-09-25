using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;

    public IMover _mover;
    private Rotator _rotator;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        _mover = new Mover (this,moveSpeed,PlayerCamera);
        _rotator = new Rotator(this);
    }


    private void Update()
    {
        _mover.Tick();
        //_rotator.Tick();
    }
}
