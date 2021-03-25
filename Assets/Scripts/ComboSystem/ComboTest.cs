using System;
using System.Collections;
using System.Collections.Generic;
using ComboSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboTest : MonoBehaviour
{
    
    public Animator Animator;
    public Combo Combo;
    private PlayerInputActions _inputActions;
    public int currentCombo=0;
    public bool inCombo;

    private void Awake()
    {
        
        //AnimatorAddParameters.TryAddingTriggerParameter(Animator,Attack_A);
        _inputActions = new PlayerInputActions();

    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.PlayerControls.A_Attack.started += HandlePressAttack_A;

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
        _inputActions.PlayerControls.A_Attack.started -= HandlePressAttack_A;
    }

    private void FixedUpdate()
    {
        var stateinfo = Animator.GetCurrentAnimatorStateInfo(0);
        foreach(var cp in Combo.ComboPieces)
        {
            if(stateinfo.IsName(cp.ComboPieceName))
            {
                inCombo=true;
                return;
            }
        }

        inCombo=false;
    }

    private void HandlePressAttack_A(InputAction.CallbackContext obj)
    {
        SendAttack_A();
    }
    public void SendAttack_A()
    {
        if(!inCombo)
        {
            Animator.Play(Combo.ComboPieces[0].ComboPieceName);
        }
        else
        {
            Animator.SetTrigger(Combo.ComboTriggerParameterName);
            currentCombo++;
            currentCombo%=Combo.ComboPieces.Length;
        }
        
    }
}
