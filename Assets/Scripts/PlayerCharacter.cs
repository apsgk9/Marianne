using System;
using CharacterInput;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float moveSpeed=4f;
    [SerializeField] private float runMoveSpeed=2f;
    [SerializeField] private float rotationSpeed=15f;
    [SerializeField] private AnimationCurve MovementVectorBlend= AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private AnimationCurve TurnRotationBlend= AnimationCurve.Linear(0,0,1,1);
    [SerializeField] public ICharacterInput _characterInput;
    [SerializeField] public ICharacterMover _characterMover;
    

    public ILocomotion _Locomotion;

    private void Awake() 
    {
        if(_characterInput==null)
        {
            _characterInput = GetComponent<ICharacterInput>();
        }
        if(_characterMover==null)
        {
            _characterMover = GetComponent<ICharacterMover>();
        }
        _Locomotion = new Locomotion (this.gameObject,moveSpeed,runMoveSpeed,rotationSpeed,
        PlayerCamera,MovementVectorBlend,TurnRotationBlend,_characterInput,_characterMover);
    }


    private void Update()
    {
        _Locomotion.Tick();
    }
    private void OnValidate()
    {
        if(_characterInput==null && GetComponent<ICharacterInput>()==null)
        {
            Debug.LogError("Player requires a Character Input.");
        }
        if(_characterMover==null && GetComponent<ICharacterMover>()==null)
        {
            Debug.LogError("Player requires a Character Mover.");
        }
        if(PlayerCamera==null)
        {
            Debug.LogError("Camera requires a Character Input.");
        }
    }
}
