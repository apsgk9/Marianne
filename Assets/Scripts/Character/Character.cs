﻿using CharacterInput;
using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private float rotationSpeed=15f;
    [SerializeField] private AnimationCurve MovementVectorBlend= AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private AnimationCurve TurnRotationBlend= AnimationCurve.Linear(0,0,1,1);
    [SerializeField] public ICharacterInput _characterInput;
    [SerializeField] public IMover _characterMover;
    [SerializeField] public Collider Collider;
    [SerializeField] private float JumpHeight;
    

    public ILocomotion _Locomotion;

    private void Awake() 
    {
        if(_characterInput==null)
        {
            _characterInput = GetComponent<ICharacterInput>();
        }
        if(_characterMover==null)
        {
            _characterMover = GetComponent<IMover>();
        }
        if(PlayerCamera==null)
        {
            if(GameObject.Find("[WorldTransform]")==false)
            {
                PlayerCamera= new GameObject("[WorldTransform]").transform;
            }
        }
        _Locomotion = new Locomotion (this.gameObject,rotationSpeed,JumpHeight,
        PlayerCamera,MovementVectorBlend,TurnRotationBlend,_characterInput,_characterMover);
    }

    private void FixedUpdate()
    {
        if(!GameStateMachine.Instance.isPaused())
        {            
            _Locomotion.Tick();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _Locomotion.CollidedWith(other);
    }
    private void OnValidate()
    {
        //if(_characterInput==null && GetComponent<ICharacterInput>()==null)
        //{
        //    Debug.LogError("Player requires a Character Input.");
        //}
        //if(_characterMover==null && GetComponent<ICharacterMover>()==null)
        //{
        //    Debug.LogError("Player requires a Character Mover.");
        //}
        //if(PlayerCamera==null)
        //{
        //    Debug.LogError("Camera requires a Character Input.");
        //}
    }


}
