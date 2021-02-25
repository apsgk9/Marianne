
using UnityEngine;
using System;
using Movement;
using CharacterProperties;

[RequireComponent(typeof(Mover))]
public class MoverSensorInterface : MonoBehaviour, IGroundSensors
{
    
    private IMover _Mover;    
    private bool _isGrounded;

    public event Action<bool> OnGroundedChange;
    
    private void Awake()
    {
        _Mover= GetComponent<IMover>();
    }

    private void FixedUpdate()
    {
        HasGroundUpdated();
    }

    private void HasGroundUpdated()
    {
        _Mover.CheckForGround();
        var result = _Mover.IsGrounded();
        if (_isGrounded != result)
        {
            OnGroundedChange?.Invoke(result);
            _isGrounded = result;
        }
    }
}
