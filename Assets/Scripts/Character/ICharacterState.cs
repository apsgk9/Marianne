using UnityEngine;

namespace CharacterProperties
{
    public interface ICharacterState
    {
        Vector3 DesiredVelocity { get; }
        Vector3 DesiredDeltaVelocity { get; }
        Vector3 ActualCurrentVelocity { get; }
        float DesiredMagnitudeSpeed { get; }
        float CharacterAnimatorSpeed { get; }
        bool CanUseStamina { get; }
        bool TryingToJump { get; }
        bool CanJump { get; }
        bool isGrounded { get; }
    }
}