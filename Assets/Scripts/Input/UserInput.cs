using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserInput : MonoBehaviour, IUserInput
{
    public static IUserInput Instance {get; set;}
    public Vector2 DirectionVector => new Vector2(Horizontal,Vertical);
    public Vector2 LastDirectionVector;
    public event Action<int> HotKeyPressed;
    public Vector2 CursorPosition => _mousePosition;
    public Vector2 CursorDeltaPosition => _cursorDeltaPosition + _analogAimPosition;
    public Vector2 LastCursorPosition;
    public float IdleThreshold =2f;
    public bool isPlayerLookIdle =>MouseIdleTimer.Activated;
    private Timer MouseIdleTimer;
    public float Vertical => _vertical;
    public float Horizontal => _horizontal;
    public bool PausePressed {get;}
    private bool _isPlayerTryingToMove;

    public Vector2 _cursorDeltaPosition;
    private Vector2 _mousePosition;
    private Vector2 _analogAimPosition;
    private float _vertical;
    private float _horizontal;
    public float AnalogAimSensitivity=15f;
    public bool RunPressed => _runPressed;
    private bool _runPressed;
    public bool JumpPressed =>_jumpPressed;
    private bool _jumpPressed;
    private float _scroll;
    public float Scroll => _scroll;

    //New actions    
    public PlayerInputActions _inputActions;
    private const int historyMaxLength=4;
    
    public string DeviceUsing=>_deviceUsing;


    private string _deviceUsing;
    private MovementHistory _verticalHistory;
    private MovementHistory _horizontalHistory;

    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastDirectionVector=DirectionVector;
        
        _inputActions = new PlayerInputActions();
        _deviceUsing="Keyboard"; //default to keyboard

        //MovementAxisHistory
        _verticalHistory= new MovementHistory(historyMaxLength);
        _horizontalHistory= new MovementHistory(historyMaxLength);
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.MovementAxis.performed += HandleMovement;
        _inputActions.Player.MovementAxis.canceled += ctx => HandleMovementCancel();

        _inputActions.Player.MouseAim.performed += HandleMouseAim;
        _inputActions.Player.MouseDeltaAim.performed += HandleMouseDeltaAim;
        _inputActions.Player.MouseDeltaAim.canceled += ctx => _cursorDeltaPosition = Vector2.zero;

        _inputActions.Player.AnalogAim.performed += HandleAnalogAim;
        _inputActions.Player.Run.started += HandleRunPressed;
        _inputActions.Player.Run.canceled += HandleRunReleased;

        
        _inputActions.Player.Jump.started += HandleJumpStart;
        _inputActions.Player.Jump.canceled += HandleJumpEnd;

        
        _inputActions.Player.Scroll.started+= HandleStartScroll; 
        _inputActions.Player.Scroll.canceled+= HandleEndScroll;
        
        InputUser.onChange+= OnDeviceChanged;

    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.MovementAxis.performed-= HandleMovement;
        _inputActions.Player.MovementAxis.canceled-= ctx=>HandleMovementCancel();

        _inputActions.Player.MouseAim.performed-= HandleMouseAim;
        _inputActions.Player.MouseDeltaAim.performed-= HandleMouseDeltaAim;
        _inputActions.Player.AnalogAim.performed-= HandleAnalogAim;
        _inputActions.Player.MouseDeltaAim.canceled-= ctx=>_cursorDeltaPosition= Vector2.zero;
        _inputActions.Player.Run.started+=HandleRunPressed;
        _inputActions.Player.Run.canceled+=HandleRunReleased;

        _inputActions.Player.Jump.started -= HandleJumpStart;
        _inputActions.Player.Jump.canceled -= HandleJumpEnd;

        _inputActions.Player.Scroll.started-= HandleStartScroll;
        _inputActions.Player.Scroll.canceled-= HandleEndScroll;

    }
    private void HandleStartScroll(InputAction.CallbackContext context)
    {
        var value= context.ReadValue<Vector2>();
        _scroll=value.y/120; //Values from mouse output in 120 increments.
    }
    private void HandleEndScroll(InputAction.CallbackContext obj)
    {
        _scroll=0f;
    }

    private void HandleJumpStart(InputAction.CallbackContext obj)
    {
        _jumpPressed=true;
    }

    private void HandleJumpEnd(InputAction.CallbackContext obj)
    {
        _jumpPressed=false;
    }

    private void OnDeviceChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if(change.ToString()=="DevicePaired" && device!=null)        
        {
            _deviceUsing=device.name;
        }
    }

    

    

    private void HandleMovementCancel()
    {
        _horizontal = 0f;
        _vertical = 0f;
    }
    
    public void Tick()
    {
        PlayerMouseIdleCheck();
        PlayerMovementIdleCheck();
        LastCursorPosition = CursorPosition;
        LastDirectionVector=DirectionVector;

        MovementHistory();
        
    }
    private void Update()
    {
        Tick();        
    }

    private void MovementHistory()
    {
        _verticalHistory.Tick(Vertical,InputHelper.DeviceInputTool.IsUsingController());
        _horizontalHistory.Tick(Horizontal,InputHelper.DeviceInputTool.IsUsingController());
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
        float verticalAverage=_verticalHistory.Average();
        float horizontalAverage=_horizontalHistory.Average();
      
        bool isMovementhere= verticalAverage > 0.0025 || horizontalAverage > 0.0025;
        return isMovementhere;
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

    private static void DeviceChange(InputDevice device,InputDeviceChange change)
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
}