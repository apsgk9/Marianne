using System.Collections;
using UnityEngine;

public class RunTransitionHandler: MonoBehaviour
{
    private int _runMultiplierTarget;
    public float RunMultiplier{get; private set;}

    private bool _isTransitioning;
    private int _runMultiplierTargetPrevious;
    public float RunTransitionTime=0.25f;
    public float StopTransitionTime=0.35f;
    private float TransitionTimeToUse;
    public AnimationCurve AnimationCurveToUse { get; private set; }
    public AnimationCurve RunTransitionCurve= AnimationCurve.Linear(0,0,1,1);
    public AnimationCurve StopTransitionCurve= AnimationCurve.Linear(0,0,1,1);
    private Coroutine _runningCouroutine;
    [HideInInspector]
    public int baseTarget=1;
    [HideInInspector]
    public int runningTarget=2;

    public virtual void Start()
    {
        RunMultiplier = 1f;
        _runMultiplierTargetPrevious = GetRunMuliplierTarget();
        _runMultiplierTarget = _runMultiplierTargetPrevious;        
    }
    public virtual void Update()
    {
        _runMultiplierTarget = GetRunMuliplierTarget();

        if (runMultiplierTargetHasChanged())
        {
            if (_runningCouroutine != null)
            {
                StopCoroutine(_runningCouroutine);
            }
            TransitionTimeToUse = GetTransitionTimeToUse();
            AnimationCurveToUse = GetAnimationCurveToUse();
            _runningCouroutine = StartCoroutine(Transition(TransitionTimeToUse,AnimationCurveToUse));
        }        
    }

    private AnimationCurve GetAnimationCurveToUse()
    {
        if(PlayerCharacterInput.Instance.RunPressed)
        {
            return RunTransitionCurve;
        }
        return StopTransitionCurve;
    }

    private float GetTransitionTimeToUse()
    {
        if(PlayerCharacterInput.Instance.RunPressed)
        {
            return RunTransitionTime;
        }
        return StopTransitionTime;
    }
    private int GetRunMuliplierTarget()
    {
        return PlayerCharacterInput.Instance.RunPressed ? runningTarget : baseTarget;
    }

    private bool runMultiplierTargetHasChanged()
    {
        if(_runMultiplierTargetPrevious!=_runMultiplierTarget)
        {
            _runMultiplierTargetPrevious=_runMultiplierTarget;
            return true;
        }
        return false;
    }

    private IEnumerator Transition(float TransitionTime,AnimationCurve AnimationCurve)
    {
        _isTransitioning=true;
        var initialMultiplier=RunMultiplier;
        var currentTime = 0f;
        while( currentTime <= TransitionTime && _isTransitioning==true)
        {
            var newTime = currentTime / TransitionTime;
            RunMultiplier= initialMultiplier + AnimationCurve.Evaluate(newTime)*(_runMultiplierTarget-initialMultiplier);
            yield return null;
            currentTime += Time.deltaTime;
        }

        if (currentTime >= TransitionTime && _isTransitioning==true)
        {
            RunMultiplier=_runMultiplierTarget;
        }
        _isTransitioning=false;        
    }

}
