using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public Vector3 Velocity;
    public float Speed;
    public Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        if( _player._Locomotion!=null)
        {
             _player._Locomotion.OnMoveChange+= UpdateMoveChange;
        }
       
    }
    private void Start()
    {
        if( _player._Locomotion!=null)
        {
             _player._Locomotion.OnMoveChange-= UpdateMoveChange;
             _player._Locomotion.OnMoveChange+= UpdateMoveChange;
        }        
    }
    private void OnDisable()
    {
        if( _player._Locomotion!=null)
        {
             _player._Locomotion.OnMoveChange-= UpdateMoveChange;
        }
        
    }

    private void UpdateMoveChange(Vector3 MoveVector)
    {
        Velocity=MoveVector;
        Speed=MoveVector.magnitude;
    }
}
