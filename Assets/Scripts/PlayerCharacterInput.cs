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
    public Vector2 CursorPosition => _mousePosition;
    public Vector2 CursorDeltaPosition => _cursorDeltaPosition + _analogAimPosition;
    public Vector2 LastCursorPosition;
    public float IdleThreshold =2f;
    public bool isPlayerLookIdle =>MouseIdleTimer.Activated;
    public bool isPlayerTryingToMove => _isPlayerTryingToMove;
    private Timer MouseIdleTimer;
    public float Vertical => _vertical;
    public float Horizontal => _horizontal;
    public bool PausePressed {get;}
    private bool _isPlayerTryingToMove;

    public Vector2 _cursorDeltaPosition;
    private Vector2 _mousePosition;
    private Vector2 _analogAimPosition;
    public float _vertical;
    public float _horizontal;
    public float AnalogAimSensitivity=15f;
    public bool RunPressed => _runPressed;
    public bool _runPressed;

    //New actions    
    public PlayerInputActions _inputActions;

    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastDirectionVector=DirectionVector;
        
        _inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.MovementAxis.performed+= HandleMovement;
        _inputActions.Player.MouseAim.performed+= HandleMouseAim;
        _inputActions.Player.MouseDeltaAim.performed+= HandleMouseDeltaAim;
        _inputActions.Player.AnalogAim.performed+= HandleAnalogAim;
        //_inputActions.Player.Run.performed+= HandleRun;
        _inputActions.Player.Run.started+=HandleRunPressed;
        _inputActions.Player.Run.canceled+=HandleRunReleased;
    }    

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.MovementAxis.performed-= HandleMovement;
        _inputActions.Player.MouseAim.performed-= HandleMouseAim;
        _inputActions.Player.MouseDeltaAim.performed-= HandleMouseDeltaAim;
        _inputActions.Player.AnalogAim.performed-= HandleAnalogAim;
        //_inputActions.Player.Run.performed-= HandleRun;
        _inputActions.Player.Run.started+=HandleRunPressed;
        _inputActions.Player.Run.canceled+=HandleRunReleased;
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
    private void LateUpdate()
    {        
        _cursorDeltaPosition= Vector2.zero;
    }

    
    private void HandleMovement(InputAction.CallbackContext context)
    {
        var value= context.ReadValue<Vector2>();
        _horizontal = value.x;
        _vertical = value.y;
    }

    private void HandleMouseAim(InputAction.CallbackContext context)
    {
        _mousePosition= context.ReadValue<Vector2>();
    }
    private void HandleMouseDeltaAim(InputAction.CallbackContext context)
    {
        _cursorDeltaPosition = context.ReadValue<Vector2>();
    }

    private void HandleAnalogAim(InputAction.CallbackContext context)
    {
        _analogAimPosition= context.ReadValue<Vector2>()*AnalogAimSensitivity;

    }
    private void HandleRunReleased(InputAction.CallbackContext obj)
    {
        _runPressed=false;
    }

    private void HandleRunPressed(InputAction.CallbackContext obj)
    {
        _runPressed=true;
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
