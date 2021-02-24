using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionDelta : MonoBehaviour
{
    public string CanRotateParameterName="CanRotate";
    public bool canRotate;
    public AnimatorControllerParameter CanRotateParamater;
    private bool _rotateparamaterexists;

    public Animator Animator { get; private set; }
    public event Action<Vector3,Quaternion> OnRootMotionChange;

    private void Awake()
    {        
        if (Animator == null)
        {
            Animator = gameObject.GetComponent<Animator>();
        }

      
    }   
    void OnAnimatorMove()
    {
        canRotate=Animator.GetBool(CanRotateParameterName);
        OnRootMotionChange?.Invoke(Animator.deltaPosition,Animator.rootRotation);
        Animator.transform.localPosition=Vector3.zero;
    }
}
