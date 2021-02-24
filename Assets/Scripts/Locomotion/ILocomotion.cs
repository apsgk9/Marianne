using System;
using UnityEngine;
using static LocomotionEnmus;

public interface ILocomotion
{
    void Tick();
    event Action<Vector3> OnMoveChange;
    event Action<float> OnMoveAnimatorSpeedChange;
    event Action<bool> OnJump;

    LocomotionMode LocomotionMode {get;}
    Vector3 DesiredCharacterVectorForward{get;}

    void ApplyRotation(Quaternion FinalRotation);
    
    bool UseMovementAngleDifference{get;set;}
}
