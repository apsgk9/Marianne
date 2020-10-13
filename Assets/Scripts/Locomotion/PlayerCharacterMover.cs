using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterMover: MonoBehaviour, ICharacterMover
{
    public CharacterController _CharacterController { get; private set; }

    private void Awake()
    {
        _CharacterController= GetComponent<CharacterController>();        
        
    }
    public void Move(Vector3 motion)
    {
        _CharacterController.Move(motion);
    }
}
