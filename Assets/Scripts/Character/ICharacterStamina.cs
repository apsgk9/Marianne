using System;
using UnityEngine;

namespace CharacterProperties
{
    namespace CharacterProperties
    {
        public interface ICharacterStamina
        {
            void Tick();
            event Action<Vector3> OnStaminaChanged;
        }
    }
}
