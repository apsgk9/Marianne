using System;
using CharacterProperties;
using UnityEngine;
using static LocomotionEnmus;

public interface ILocomotion
{
    void Tick();
    event Action<Vector3> OnMoveChange;
    event Action<float> OnMoveAnimatorSpeedChange;
    event Action<bool> OnTryingToJump;
    event Action<bool> OnCanJump;
	event Action<Vector3> OnLand;
    event Action<State> OnStateChange;
    void Jump(float Height);
    LocomotionMode LocomotionMode {get;}
    Vector3 DesiredCharacterVectorForward{get;}
    void ApplyRotation(Quaternion FinalRotation);    
    bool UseMovementAngleDifference{get;set;}
}
