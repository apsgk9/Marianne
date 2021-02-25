using System;

namespace CharacterProperties
{
    public interface IGroundSensors
    {
        event Action<bool> OnGroundedChange;
    }
}