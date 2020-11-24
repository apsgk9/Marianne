using System;
using UnityEngine;

namespace CharacterProperties
{
    public interface ICharacterStamina
    {
        bool HasStamina();
        bool IsStaminaBeingUsed{get;set;}
        event Action<float> OnStaminaChanged;
        float ChangeStamina(float newStamina);
        float AddStamina(float changeToAdd);
        float GetCurrentStamina();
    }
}
