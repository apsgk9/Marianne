using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

public class PlayerCharacterMover : Mover
{
    public override void SetVelocity(Vector3 _velocity)
    {
        rig.velocity=_velocity;
    }
}

