using System;
using UnityEngine;

public interface IPlayerInput
{
    event Action MoveModeTogglePressed;
    event Action<int> HotKeyPressed;
    float Vertical {get;}
    float Horizontal {get;}
    void Tick();

    bool PausePressed {get;}
    Vector2 CursorPosition { get;}
    bool isPlayerLookIdle { get; }
}