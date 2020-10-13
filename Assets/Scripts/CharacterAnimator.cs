using System;
using System.Collections.Generic;
using CharacterInput;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator Animator;
    public PlayerState PlayerState;
    public string SpeedParameterName = "Speed";
    private float _compositeSpeedValue;
    private Vector2 _rawDirection;
    public string MovementPressedParameterName ="MovementPressed";
    public string UsingControllerParameterName ="UsingController";
    private static Vector2 _previousMovmementAxis;
    private ICharacterInput _characterInput;

    private void Awake()
    {   
        if(_characterInput==null)
        {
            _characterInput = GetComponentInParent<ICharacterInput>();
        }
        _previousMovmementAxis=Vector2.zero;
    }
    public void Update()
    {
        
        SpeedCalculations();

        Animator.SetFloat(SpeedParameterName, _compositeSpeedValue);

        Animator.SetBool(MovementPressedParameterName, GetMovementPressed());
        
        Animator.SetBool(UsingControllerParameterName, InputHelper.DeviceInputTool.IsUsingController());
    }
    private bool GetMovementPressed()
    {
        return _characterInput.IsThereMovement();
    }
    

    private float ReturnDeltaMovement()
    {
        var movementAxis=new Vector2(_characterInput.MovementHorizontal(), _characterInput.MovementVertical());
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
        _compositeSpeedValue=PlayerState.AnimatorSpeed;
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
