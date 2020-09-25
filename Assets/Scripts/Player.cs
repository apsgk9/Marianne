using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;
    [SerializeField] private float rotationSpeed=4f;

    public ILocomotion _Locomotion;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        _Locomotion = new Locomotion (this,moveSpeed,rotationSpeed,PlayerCamera);
    }


    private void Update()
    {
        _Locomotion.Tick();
    }
}
