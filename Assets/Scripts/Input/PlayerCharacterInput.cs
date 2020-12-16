using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInput
{
    public class PlayerCharacterInput : MonoBehaviour, ICharacterInput
    {        

        public float MovementHorizontal()
        {
            return UserInput.Instance.Horizontal;
        }

        public float MovementVertical()
        {
            return UserInput.Instance.Vertical;
        }
        public bool IsRunning()
        {
            return UserInput.Instance.RunPressed;
        }
        public bool IsThereMovement()
        {
            return UserInput.Instance.IsThereMovement();
        }
        public bool AttemptingToJump()
        {
             return UserInput.Instance.JumpPressed;
        }
    }
}
