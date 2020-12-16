using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboSystem : MonoBehaviour
{
    
    public Animator Animator;
    private string Attack_A="Attack_A";
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        
        AnimatorAddParameters.TryAddingTriggerParameter(Animator,Attack_A);
        _inputActions = new PlayerInputActions();

    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.A_Attack.started += HandlePressAttack_A;


    }
    private void OnDisable()
    {
        Deregister();

    }
    private void OnDestroy()
    {
        Deregister();
    }
    private void Deregister()
    {
        _inputActions.Disable();
        _inputActions.Player.A_Attack.started -= HandlePressAttack_A;
    }

    private void HandlePressAttack_A(InputAction.CallbackContext obj)
    {
        SendAttack_A();
    }
    public void SendAttack_A()
    {
        Animator.SetTrigger(Attack_A);
    }
}
