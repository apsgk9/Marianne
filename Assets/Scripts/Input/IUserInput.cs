using System;
using UnityEngine;

public interface IUserInput
{
    event Action<int> HotKeyPressed;
    float Vertical {get;}
    float Horizontal {get;}
    float Scroll {get;}
    void Tick();
    bool PausePressed {get;}
    bool JumpPressed {get;}
    bool RunPressed {get;}
    Vector2 CursorPosition { get;}
    Vector2 CursorDeltaPosition { get;}
    bool isPlayerLookIdle { get; }
    bool IsThereMovement();
    string DeviceUsing{get;}
}