using System;
using UnityEngine;

public interface ILocomotion
{
    void Tick();
    Vector3 DeltaMovement{get;}
    event Action<Vector3> OnMoveChange;
}
