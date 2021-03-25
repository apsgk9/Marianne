using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionDelta : MonoBehaviour
{
    
    [SerializeField]
    private CharacterAnimatorNamingList  CharacterAnimatorNamingList;
    public bool canRotate;
    public Animator Animator { get; private set; }

    public event Action<Vector3> OnRootPositionChange;

    private void Awake()
    {        
        if (Animator == null)
        {
            Animator = gameObject.GetComponent<Animator>();
        }
      
    }   
    void OnAnimatorMove()
    {
        canRotate=Animator.GetBool(CharacterAnimatorNamingList.CanRotateParameterName);
        OnRootPositionChange?.Invoke(Animator.deltaPosition);
        Animator.transform.localPosition=Vector3.zero;
    }
}
