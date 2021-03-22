using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorNamingList : ScriptableObject
{
    public string SpeedParameterName = "Speed";
    public string MovementPressedParameterName ="MovementPressed";
    public string UsingControllerParameterName ="UsingController";
    public string ControllerDeltaParameterName ="ControllerDelta";
    public string CharacterHasStaminaParameterName ="HasStamina";
    public string JumpTriggerParameterName ="Jump";
    public string IsJumpingParameterName ="IsJumping";
    public string isGroundedParameterName ="isGrounded";
    public string InterruptableParameterName ="Interruptable";
}
