using System;
using System.Collections;
using System.Collections.Generic;
using ComboSystem;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboTest : MonoBehaviour, IRotateDesiredForwardEvent
{
    
    public Animator Animator;
    public Combo Combo;
    private PlayerInputActions _inputActions;
    [SerializeField]
    private RootMotionDelta _rootMotionDelta;
    public int currentCombo=0;
    public string currentState;
    public bool inCombo;
    public event Action OnCallDesiredForwardRotationChange;
    //
    private AnimatorStateInfo _previousCurrentStateInfo;
    private AnimatorStateInfo _previousNextStateInfo;
    private bool _previousIsAnimatorTransitioning;
    private AnimatorStateInfo _currentStateInfo;
    private AnimatorStateInfo _nextStateInfo;
    private bool _isAnimatorTransitioning;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        currentCombo=0;
        currentState="NOTINSTATE";

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
        if(inCombo)
        {
            _rootMotionDelta.canRotate=false;
        }
        else
        {
            currentCombo=-1;
            currentState="NOTINSTATE";
        }
        
    }

    private void OnAnimatorMove()
    {
        CacheAnimatorState();
        CalculateIfInCombo(_currentStateInfo);
        if(inCombo)
        {
            CalculateCurrentCombo();
        }
    }

    private void CalculateCurrentCombo()
    {
        if(_currentStateInfo.fullPathHash != _previousCurrentStateInfo.fullPathHash )
        {
            currentCombo++;
            currentCombo%=Combo.ComboPieces.Length+1;
            if(currentCombo==0)
                currentCombo++;
            currentState=Combo.ComboPieces[currentCombo-1].ComboPieceName;
            OnCallDesiredForwardRotationChange?.Invoke();

        }
    }


    // Called at the start of FixedUpdate to record the current state of the base layer of the animator.
    void CacheAnimatorState()
        {
            _previousCurrentStateInfo = _currentStateInfo;
            _previousNextStateInfo = _nextStateInfo;
            _previousIsAnimatorTransitioning = _isAnimatorTransitioning;

            _currentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            _nextStateInfo = Animator.GetNextAnimatorStateInfo(0);
            _isAnimatorTransitioning = Animator.IsInTransition(0);
        }

    private void CalculateIfInCombo(AnimatorStateInfo stateinfo)
    {
        foreach (var cp in Combo.ComboPieces)
        {
            if (stateinfo.IsName(cp.ComboPieceName))
            {
                inCombo = true;
                return;
            }
        }

        inCombo = false;
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
            //currentState=Combo.ComboPieces[0].ComboPieceName;
            currentCombo=0;
        }
        else
        {
            Animator.SetTrigger(Combo.ComboTriggerParameterName);
        }
        
    }
}
