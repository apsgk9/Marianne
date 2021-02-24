using System;
using UnityEngine;

public interface ICharacterMover
{
    bool UseGravity {get; set;}
    void OnUpdateAnimatorMove(Vector3 motion);
    void SetConstantMoveUpdate(Vector3 motion);
    void AddExtraMotion(Vector3 motion);
    void AddVelocity(Vector3 vInput);
    void SetVelocity(Vector3 vInput);
    void SetGroundVelocity(float x,float z);
    void Jump(float height);
    Vector3 TotalVector{get; set;}
}
