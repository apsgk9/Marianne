using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionDelta : MonoBehaviour
{
    public GameObject PlayerGameobject;
    public Animator Animator { get; private set; }
    public Player Player { get; private set; }
    public event Action<Vector3,Quaternion> OnRootMotionChange;

    private void Awake()
    {
        Animator = GetComponent<Animator>();    
        Player = GetComponentInParent<Player>();
    }   
    void OnAnimatorMove()
    {
        OnRootMotionChange?.Invoke(Animator.deltaPosition,Animator.rootRotation);
        //PlayerGameobject.transform.rotation = Animator.rootRotation;
        //PlayerGameobject.transform.position += Animator.deltaPosition;
        Animator.transform.localPosition=Vector3.zero;
    }
    private void OnValidate()
    {
        
        if(PlayerGameobject==null)
        {
            Debug.LogWarning("PlayerState is missing for PlayerAnimator.");
        }
    }
}
