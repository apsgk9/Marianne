using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public AnimatorStateInfo stateInfo;
    public Animator Animator;

    public string SpeedParameterName = "Speed";
    private float _rawSpeedValue;
    private Vector2 _rawDirection;

    //private static readonly float Speed = Animator.StringToHash("Speed");

    private void Awake()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        _rawDirection = new Vector2(PlayerCharacterInput.Instance.Horizontal,PlayerCharacterInput.Instance.Vertical);
        _rawSpeedValue= _rawDirection.magnitude;
        
        Animator.SetFloat(SpeedParameterName,_rawSpeedValue);
    }
    private void OnValidate()
    {
        if(Animator==null)
        {
            Debug.LogWarning("PlayerAnimator must have an animator.");
        }
        
    }
}
