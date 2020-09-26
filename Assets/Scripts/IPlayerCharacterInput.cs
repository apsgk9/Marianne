using System;
using UnityEngine;

public interface IPlayerCharacterInput
{
    event Action MoveModeTogglePressed;
    event Action<int> HotKeyPressed;
    float Vertical {get;}
    float Horizontal {get;}
    void Tick();

    bool PausePressed {get;}
    Vector2 CursorPosition { get;}
    bool isPlayerLookIdle { get; }
    bool isPlayerTryingToMove { get; }
    bool IsThereMovement();
}