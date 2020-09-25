using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public static IPlayerInput Instance {get; set;}
    public event Action MoveModeTogglePressed;
    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");
    public Vector2 DirectionMagnitude => new Vector2(Horizontal,Vertical);
    public Vector2 LastDirectionMagnitude;
    public event Action<int> HotKeyPressed;
    public bool PausePressed {get;}
    public Vector2 CursorPosition => Input.mousePosition;
    public Vector2 LastCursorPosition;
    public float IdleThreshold =2f;
    public bool isPlayerLookIdle =>MouseIdleTimer.Activated;

    public bool isPlayerTryingToMove => _isPlayerTryingToMove;
    private bool _isPlayerTryingToMove;

    private Timer MouseIdleTimer;

    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastCursorPosition=Input.mousePosition;
        LastDirectionMagnitude=DirectionMagnitude;
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
        PlayerMovementIdleCheck();
        LastCursorPosition = CursorPosition;
        LastDirectionMagnitude=DirectionMagnitude;
    }

    public bool IsThereMovement()
    {
        return Vertical > Mathf.Epsilon || Horizontal > Mathf.Epsilon;
    }

    private bool IsThereDifferenceInMovement()
    {
        return LastDirectionMagnitude != DirectionMagnitude;
    }
    private void PlayerMovementIdleCheck()
    {
        _isPlayerTryingToMove = IsThereDifferenceInMovement() ? true: false;
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
