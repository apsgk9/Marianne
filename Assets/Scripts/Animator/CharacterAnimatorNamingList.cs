using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorNamingList : ScriptableObject
{
    [Header("ParameterList")]
    public string SpeedParameterName = "Speed";
    public string MovementPressedParameterName ="MovementPressed";
    public string UsingControllerParameterName ="UsingController";
    public string ControllerDeltaParameterName ="ControllerDelta";
    public string CharacterHasStaminaParameterName ="HasStamina";
    public string JumpTriggerParameterName ="Jump";
    public string IsJumpingParameterName ="IsJumping";
    public string isGroundedParameterName ="isGrounded";
    public string InterruptableParameterName ="Interruptable";
    public string CanRotateParameterName = "CanRotate";
    public string NormalizedTimeParameterName = "NormalizedTime";

    [Header("StateList")]
    public string IdleStateName = "Idle";
    public string RunStateName = "Run";
    public string WalkStateName = "Walk";
    public string StartRunStateName = "StartRun";
    public string StopLeftStateName = "StopLeft";
    public string StopRightStateName = "StopRight";
}
