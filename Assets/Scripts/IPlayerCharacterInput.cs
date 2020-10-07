using System;
using UnityEngine;

public interface IPlayerCharacterInput
{
    event Action<int> HotKeyPressed;
    float Vertical {get;}
    float Horizontal {get;}
    void Tick();
    bool PausePressed {get;}
    bool RunPressed {get;}
    Vector2 CursorPosition { get;}
    Vector2 CursorDeltaPosition { get;}
    bool isPlayerLookIdle { get; }
    bool IsThereMovement();
}