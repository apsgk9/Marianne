using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterInput : MonoBehaviour, IPlayerCharacterInput
{
    public static IPlayerCharacterInput Instance {get; set;}
    public event Action MoveModeTogglePressed;
    public Vector2 DirectionVector => new Vector2(Horizontal,Vertical);
    public Vector2 LastDirectionVector;
    public event Action<int> HotKeyPressed;
    public bool PausePressed {get;}
    public Vector2 CursorPosition => Input.mousePosition;
    public Vector2 LastCursorPosition;
    public float IdleThreshold =2f;
    public bool isPlayerLookIdle =>MouseIdleTimer.Activated;
    public bool isPlayerTryingToMove => _isPlayerTryingToMove;
    private bool _isPlayerTryingToMove;
    private Timer MouseIdleTimer;
    public float Vertical => _vertical;
    public float Horizontal => _horizontal;
    public float _vertical;
    public float _horizontal;

    //New actions    
    public PlayerInputActions _inputActions;

    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastCursorPosition=Input.mousePosition;
        LastDirectionVector=DirectionVector;
        
        _inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Movement.performed+= HandleMovement;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Movement.performed-= HandleMovement;
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
        LastDirectionVector=DirectionVector;
    }

    
    private void HandleMovement(InputAction.CallbackContext context)
    {
        var value= context.ReadValue<Vector2>();
        Debug.Log("value:"+value);
        _horizontal = value.x;
        _vertical = value.y;
    }


    public bool IsThereMovement()
    {
        return Vertical > Mathf.Epsilon || Horizontal > Mathf.Epsilon;
    }

    private bool IsThereDifferenceInMovement()
    {
        return LastDirectionVector != DirectionVector;
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
