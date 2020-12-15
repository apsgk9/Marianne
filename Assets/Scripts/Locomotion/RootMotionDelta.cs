using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionDelta : MonoBehaviour
{
    public string CanRotateParameter="CanRotate";
    public bool canRotate;
    public AnimatorControllerParameter CanRotateParamater;
    private bool _rotateparamaterexists;

    public Animator Animator { get; private set; }
    public event Action<Vector3,Quaternion> OnRootMotionChange;

    private void Awake()
    {
        CheckisHasRotateParameter();
    }   
    void OnAnimatorMove()
    {
        canRotate=Animator.GetBool(CanRotateParameter);
        OnRootMotionChange?.Invoke(Animator.deltaPosition,Animator.rootRotation);
        Animator.transform.localPosition=Vector3.zero;
    }


    private void CheckisHasRotateParameter()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        AnimatorController animatorController = (AnimatorController)Animator.runtimeAnimatorController;

        if(CanRotateParamater==null)
        {
            for (int i = 0; i < Animator.parameterCount; i++)
            {
                AnimatorControllerParameter tempParemeter=Animator.GetParameter(i);
                if (tempParemeter.name == CanRotateParameter)
                {
                    CanRotateParamater=tempParemeter;
                    _rotateparamaterexists=true;
                    break;
                }
            }
        }
        

        if (_rotateparamaterexists==false)
        {
            AnimatorControllerParameter parameter = new AnimatorControllerParameter();
            parameter.type = AnimatorControllerParameterType.Bool;
            parameter.name = CanRotateParameter;
            parameter.defaultBool = true;
            animatorController.AddParameter(parameter);
        }
    }
}
