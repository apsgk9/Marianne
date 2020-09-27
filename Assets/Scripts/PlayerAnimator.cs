using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RunTransitionHandler))]
public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator;
    public PlayerState PlayerState;
    public string SpeedParameterName = "Speed";
    private float _compositeSpeedValue;
    private Vector2 _rawDirection;
    public RunTransitionHandler _runTransitionHandler { get; private set;}
    public float AnimationSpeedTweaker=0.5f;
    public bool UseCurves=true;
    private void Awake()
    {
        _runTransitionHandler= GetComponent<RunTransitionHandler>();
    }
    public void Update()
    {
        if(UseCurves)
        {
            CurveCalculations();
        }
        else
        {
            SpeedCalculations();
        }

        Animator.SetFloat(SpeedParameterName, _compositeSpeedValue);
    }

    private void SpeedCalculations()
    {
        _compositeSpeedValue=PlayerState.Speed*AnimationSpeedTweaker;
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
