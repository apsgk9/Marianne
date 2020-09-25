using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public static IPlayerInput Instance {get; set;}
    public event Action MoveModeTogglePressed;
    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");
    public float MouseX => Input.GetAxis("Mouse X");
    public event Action<int> HotKeyPressed;
    public bool PausePressed {get;}
    public Vector2 CursorPosition => Input.mousePosition;
    public Vector2 LastCursorPosition;
    public float IdleThreshold =2f;
    public bool isPlayerLookIdle =>MouseIdleTimer.Activated;
    private Timer MouseIdleTimer;

    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastCursorPosition=Input.mousePosition;
    }

    public void Tick()
    {
        if (MoveModeTogglePressed != null && Input.GetKeyDown(KeyCode.Minus))
        {
            MoveModeTogglePressed();
        }

        HotKeyCheck();
    }
    private void Update()
    {        
        PlayerMouseIdleCheck();
        LastCursorPosition= CursorPosition;
    }

    private void PlayerMouseIdleCheck()
    {
        MouseIdleTimer.Tick();
        if(hasMouseMoved())
        {
            MouseIdleTimer.ResetTimer();
        }
    }

    private bool hasMouseMoved()
    {
        return LastCursorPosition != CursorPosition;
    }

    private void HotKeyCheck()
    {
        if (HotKeyPressed == null)
        {
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                HotKeyPressed(i);
            }
        }
    }
}
