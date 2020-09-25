using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;

    public IMover _Locomotion;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        _Locomotion = new Mover (this,moveSpeed,PlayerCamera);
    }


    private void Update()
    {
        _Locomotion.Tick();
    }
}
