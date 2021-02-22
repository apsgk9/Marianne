using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserInput : Singleton<UserInput>, IUserInput
{
    public Vector2 DirectionVector => new Vector2(Horizontal, Vertical);
    public Vector2 LastDirectionVector;
    public event Action<int> HotKeyPressed;
    public Vector2 CursorPosition => _mousePosition;
    public Vector2 CursorDeltaPosition => CalculateCursorDeltaPosition();
    public Vector2 LastCursorPosition;
    public float IdleThreshold = 2f;
    public bool isPlayerLookIdle => MouseIdleTimer.Activated;
    private Timer MouseIdleTimer;
    public float Vertical => _vertical;
    public float Horizontal => _horizontal;
    private bool _isPlayerTryingToMove;

    public Vector2 _cursorDeltaPosition;
    private Vector2 _mousePosition;
    private Vector2 _analogAimPosition;
    private float _vertical;
    private float _horizontal;
    public bool RunPressed => _runPressed;
    private bool _runPressed;
    public bool JumpPressed => _jumpPressed;
    private bool _jumpPressed;

    
    private float _scroll;
    public float Scroll => _scroll;

    //New actions    
    public PlayerInputActions PlayerInputActions;
    private const int historyMaxLength = 4;

    public string DeviceUsing => _deviceUsing;


    private string _deviceUsing;
    private MovementHistory _verticalHistory;
    private MovementHistory _horizontalHistory;

    private void Awake()
    {
        //Instance = this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastDirectionVector = DirectionVector;

        PlayerInputActions = new PlayerInputActions();
        _deviceUsing = "Keyboard"; //default to keyboard

        //MovementAxisHistory
        _verticalHistory = new MovementHistory(historyMaxLength);
        _horizontalHistory = new MovementHistory(historyMaxLength);
    }
    private void OnEnable()
    {
        PlayerInputActions.Enable();
        PlayerInputActions.PlayerControls.MovementAxis.performed += HandleMovement;
        PlayerInputActions.PlayerControls.MovementAxis.canceled += ctx => HandleMovementCancel();

        PlayerInputActions.PlayerControls.MouseAim.performed += HandleMouseAim;
        PlayerInputActions.PlayerControls.MouseDeltaAim.performed += HandleMouseDeltaAim;
        PlayerInputActions.PlayerControls.MouseDeltaAim.canceled += ctx => _cursorDeltaPosition = Vector2.zero;

        PlayerInputActions.PlayerControls.AnalogAim.performed += HandleAnalogAim;
        PlayerInputActions.PlayerControls.Run.started += HandleRunPressed;
        PlayerInputActions.PlayerControls.Run.canceled += HandleRunReleased;


        PlayerInputActions.PlayerControls.Jump.started += HandleJumpStart;
        PlayerInputActions.PlayerControls.Jump.canceled += HandleJumpEnd;


        PlayerInputActions.PlayerControls.Scroll.started += HandleStartScroll;
        PlayerInputActions.PlayerControls.Scroll.canceled += HandleEndScroll;

        InputUser.onChange += OnDeviceChanged;

    }
    private void OnDisable()
    {
        PlayerInputActions.Disable();
        PlayerInputActions.PlayerControls.MovementAxis.performed -= HandleMovement;
        PlayerInputActions.PlayerControls.MovementAxis.canceled -= ctx => HandleMovementCancel();

        PlayerInputActions.PlayerControls.MouseAim.performed -= HandleMouseAim;
        PlayerInputActions.PlayerControls.MouseDeltaAim.performed -= HandleMouseDeltaAim;
        PlayerInputActions.PlayerControls.AnalogAim.performed -= HandleAnalogAim;
        PlayerInputActions.PlayerControls.MouseDeltaAim.canceled -= ctx => _cursorDeltaPosition = Vector2.zero;
        PlayerInputActions.PlayerControls.Run.started += HandleRunPressed;
        PlayerInputActions.PlayerControls.Run.canceled += HandleRunReleased;

        PlayerInputActions.PlayerControls.Jump.started -= HandleJumpStart;
        PlayerInputActions.PlayerControls.Jump.canceled -= HandleJumpEnd;

        PlayerInputActions.PlayerControls.Scroll.started -= HandleStartScroll;
        PlayerInputActions.PlayerControls.Scroll.canceled -= HandleEndScroll;

    }


    #region Tick
    private void Update()
    {
        if(GameManager.Instance.isPaused==false)
        {            
            Tick();
        }
    }

    public void Tick()
    {
        PlayerMouseIdleCheck();
        PlayerMovementIdleCheck();
        LastCursorPosition = CursorPosition;
        LastDirectionVector = DirectionVector;

        MovementHistory();
    }
    private void MovementHistory()
    {
        _verticalHistory.Tick(Vertical, InputHelper.DeviceInputTool.IsUsingController());
        _horizontalHistory.Tick(Horizontal, InputHelper.DeviceInputTool.IsUsingController());
    }
    
    #endregion
    public void EnableMenuControls()
    {        
        PlayerInputActions.MenuControls.Enable();
        PlayerInputActions.PlayerControls.Disable();
    }

    public void EnableGameplayControls()
    {
        PlayerInputActions.MenuControls.Disable();
        PlayerInputActions.PlayerControls.Enable();
    }
    #region HandleEvents

    private void HandleMovement(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _horizontal = value.x;
        _vertical = value.y;
    }
    private void HandleStartScroll(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _scroll = value.y / 120; //Values from mouse output in 120 increments.
    }
    private void HandleEndScroll(InputAction.CallbackContext obj)
    {
        _scroll = 0f;
    }

    private void HandleJumpStart(InputAction.CallbackContext obj)
    {
        _jumpPressed = true;
    }

    private void HandleJumpEnd(InputAction.CallbackContext obj)
    {
        _jumpPressed = false;
    }

    private void OnDeviceChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change.ToString() == "DevicePaired" && device != null)
        {
            _deviceUsing = device.name;
            Debug.Log("USING: " + _deviceUsing);
        }
        //PS4 "DualShock4GamepadHID"
        //PC: "Keyboard" or "Mouse"
    }

    

    private void HandleMouseAim(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }
    private void HandleMouseDeltaAim(InputAction.CallbackContext context)
    {
        _cursorDeltaPosition = context.ReadValue<Vector2>();
    }

    private void HandleAnalogAim(InputAction.CallbackContext context)
    {
        
        //_analogAimPosition = context.ReadValue<Vector2>() * 15f;
        
        Vector2 RawAnalogAim= context.ReadValue<Vector2>();
        
        _analogAimPosition = 
         new Vector2(GameManager.Instance.UserSettings.ControllerXAxisSensitivity*RawAnalogAim.x*5,
                    GameManager.Instance.UserSettings.ControllerYAxisSensitivity*RawAnalogAim.y*5);
                    

    }
    private void HandleRunReleased(InputAction.CallbackContext obj)
    {
        _runPressed = false;
    }

    private void HandleRunPressed(InputAction.CallbackContext obj)
    {
        _runPressed = true;
    }


    private void HandleMovementCancel()
    {
        _horizontal = 0f;
        _vertical = 0f;
    }

    #endregion

    #region Calculations
    private Vector2 CalculateCursorDeltaPosition()
    {
        return _cursorDeltaPosition + _analogAimPosition;
    }    

    public bool IsThereMovement()
    {
        float verticalAverage = _verticalHistory.Average();
        float horizontalAverage = _horizontalHistory.Average();

        bool isMovementhere = verticalAverage > 0.0025 || horizontalAverage > 0.0025;
        return isMovementhere;
    }

    private bool IsThereDifferenceInMovement()
    {
        return LastDirectionVector != DirectionVector;
    }
    private void PlayerMovementIdleCheck()
    {
        _isPlayerTryingToMove = IsThereDifferenceInMovement() ? true : false;
    }

    private void PlayerMouseIdleCheck()
    {
        MouseIdleTimer.Tick();
        if (hasMouseMoved())
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

    #endregion

    #region MiscMethods
    private static void DeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                Debug.Log("Added: " + device.name);
                break;
            case InputDeviceChange.Disconnected:
                // Device got unplugged.
                Debug.Log("Disconnected: " + device.name);
                break;
            case InputDeviceChange.Reconnected:
                // Plugged back in.
                Debug.Log("Reconnected: " + device.name);
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                Debug.Log("Removed: " + device.name);
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }

    }
    #endregion
}
