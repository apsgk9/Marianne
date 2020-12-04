using System;
using UnityEngine;

public interface ICharacterMover
{
    bool UseGravity {get; set;}
    void Move(Vector3 motion);
    void AddExtraMotion(Vector3 motion);

    Vector3 TotalVector{get; set;}
}
