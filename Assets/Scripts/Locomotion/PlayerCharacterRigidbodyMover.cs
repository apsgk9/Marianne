
using CharacterProperties;
using UnityEngine;
using System;
using Movement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerCharacterRigidbodyMover : Mover, IGroundSensors
{
    //------------------

    //------------------
    private bool _isGrounded;

    public bool isGrounded => throw new NotImplementedException();

    public event Action<bool> OnGroundedChange;

    private void FixedUpdate()
    {
        HasGroundUpdated();
    }

    private void HasGroundUpdated()
    {
        var result = IsGrounded();
        if (_isGrounded != result)
        {
            OnGroundedChange?.Invoke(_isGrounded);
            _isGrounded = result;
        }
    }
}
