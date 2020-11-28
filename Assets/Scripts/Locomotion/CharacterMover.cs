using System;
using UnityEngine;

public interface ICharacterMover
{
    bool UseGravity {get; set;}
    void Move(Vector3 motion);
}
