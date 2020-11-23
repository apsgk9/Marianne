using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionDelta : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public event Action<Vector3,Quaternion> OnRootMotionChange;

    private void Awake()
    {
        Animator = GetComponent<Animator>();    
    }   
    void OnAnimatorMove()
    {
        OnRootMotionChange?.Invoke(Animator.deltaPosition,Animator.rootRotation);
        Animator.transform.localPosition=Vector3.zero;
    }
}
