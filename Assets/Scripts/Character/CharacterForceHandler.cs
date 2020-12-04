using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterForceHandler : MonoBehaviour
{
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController= GetComponent<CharacterController>();
    }
}