using System;
using UnityEngine;

namespace CharacterProperties
{
    public interface ICharacterStamina
    {
        bool HasStamina();
        bool IsStaminaBeingUsed{get;set;}
        bool CanUse();
        
        //current,min,max
        event Action<float,float,float> OnStaminaChanged;
        float ChangeStamina(float newStamina);
        float AddStamina(float changeToAdd);
        float GetCurrentStamina();
    }
}
