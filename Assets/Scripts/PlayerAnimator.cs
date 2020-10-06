﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RunTransitionHandler))]
public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator;
    public PlayerState PlayerState;
    public string SpeedParameterName = "Speed";
    public string ChangeInVelocityParameterName = "ChangeInVelocity";
    public string DeltaVelocityParameterName = "DeltaVelocity";
    private float _compositeSpeedValue;
    private Vector2 _rawDirection;
    public RunTransitionHandler _runTransitionHandler { get; private set;}
    public string ControllerDeltaParameterName ="ControllerDelta";

    public float AnimationSpeedTweaker=0.5f;
    public bool UseCurves=true;
    private static Vector2 _previousMovmementAxis;

    private void Awake()
    {
        _runTransitionHandler= GetComponent<RunTransitionHandler>();
        _previousMovmementAxis=Vector2.zero;
    }
    public void Update()
    {
        if (UseCurves)
        {
            CurveCalculations();
        }
        else
        {
            SpeedCalculations();
        }

        Animator.SetFloat(SpeedParameterName, _compositeSpeedValue);
        Animator.SetBool(ChangeInVelocityParameterName, ChangeInVelocity());
        Animator.SetFloat(DeltaVelocityParameterName, PlayerState.DeltaVelocity.magnitude);

        float _ControllerMovementAxisDelta = NewMethod();
        Animator.SetFloat(ControllerDeltaParameterName, _ControllerMovementAxisDelta);
    }

    private static float NewMethod()
    {
        var movementAxis=new Vector2(PlayerCharacterInput.Instance.Horizontal, PlayerCharacterInput.Instance.Vertical);
        float delta= (movementAxis-_previousMovmementAxis).magnitude;
        

        _previousMovmementAxis=movementAxis;
        return delta;
    }

    private bool ChangeInVelocity()
    {
        return !(PlayerState.DeltaVelocity.magnitude<Mathf.Epsilon);
    }

    private void SpeedCalculations()
    {
        _compositeSpeedValue=PlayerState.AnimatorSpeed*AnimationSpeedTweaker;
    }

    private void CurveCalculations()
    {
        _rawDirection = new Vector2(PlayerCharacterInput.Instance.Horizontal, PlayerCharacterInput.Instance.Vertical);
        _compositeSpeedValue = _rawDirection.magnitude * _runTransitionHandler.RunMultiplier;
    }

    private void OnValidate()
    {
        if(Animator==null)
        {
            Debug.LogWarning("PlayerAnimator must have an animator.");
        }
        if(PlayerState==null)
        {
            Debug.LogWarning("PlayerState is missing for PlayerAnimator.");
        }
    }
}
