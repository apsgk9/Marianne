using System;
using UnityEngine;

[RequireComponent(typeof(RunTransitionHandler))]
public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;
    [SerializeField] private float runMoveSpeed=2f;
    [SerializeField] private float rotationSpeed=4f;
    [SerializeField] private RunTransitionHandler RunTransitionHandlerInput;
    [SerializeField] private float Speed;
    

    public ILocomotion _Locomotion;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        _Locomotion = new Locomotion (this,moveSpeed,runMoveSpeed,rotationSpeed,PlayerCamera,RunTransitionHandlerInput);
    }


    private void Update()
    {
        _Locomotion.Tick();
    }
}
