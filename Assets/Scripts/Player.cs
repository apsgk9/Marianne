using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;
    [SerializeField] private float runMoveSpeed=2f;
    [SerializeField] private float rotationSpeed=4f;
    [SerializeField] private AnimationCurve MovementVectorBlend= AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private AnimationCurve TurnRotationBlend= AnimationCurve.Linear(0,0,1,1);
    

    public ILocomotion _Locomotion;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        _Locomotion = new Locomotion (this,moveSpeed,runMoveSpeed,rotationSpeed,
        PlayerCamera,MovementVectorBlend,TurnRotationBlend);
    }


    private void Update()
    {
        _Locomotion.Tick();
    }
}
