using System;
using UnityEngine;


public interface ILocomotion
{
    void Tick();
    event Action<Vector3> OnMoveChange;
    event Action<float> OnMoveAnimatorSpeedChange;
}
