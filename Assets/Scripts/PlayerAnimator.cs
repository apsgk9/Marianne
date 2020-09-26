using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator;

    public string SpeedParameterName = "Speed";

    //private static readonly float Speed = Animator.StringToHash("Speed");
    
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speedValue= Mathf.Abs(PlayerCharacterInput.Instance.Horizontal)+Mathf.Abs(PlayerCharacterInput.Instance.Vertical);
        Animator.SetFloat(SpeedParameterName,speedValue);
    }
    private void OnValidate()
    {
        if(Animator==null)
        {
            Debug.LogWarning("PlayerAnimator must have an animator.");
        }
        
    }
}
