using System;

public interface ICheckGrounded
{
    event Action<bool> OnGroundedChange;
    bool isGrounded {get;}
    float DistancetoGround{get;}
}
