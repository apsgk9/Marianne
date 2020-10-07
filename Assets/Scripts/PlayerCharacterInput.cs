using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerCharacterInput : MonoBehaviour, IPlayerCharacterInput
{
    public static IPlayerCharacterInput Instance {get; set;}
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
    public float _vertical;
    public float _horizontal;
    public float AnalogAimSensitivity=15f;
    public bool RunPressed => _runPressed;
    public bool _runPressed;

    //New actions    
    public PlayerInputActions _inputActions;
    private LinkedList<float> verticalHistory= new LinkedList<float>();
    private LinkedList<float> horizontalHistory= new LinkedList<float>();
    private const int historyMaxLength=4;
    
    public string DeviceUsing=>_deviceUsing;
    private string _deviceUsing;
    private void Awake()
    {
        Instance=this;
        MouseIdleTimer = new Timer(IdleThreshold);
        LastDirectionVector=DirectionVector;
        
        _inputActions = new PlayerInputActions();
        _deviceUsing="Keyboard"; //default to keyboard
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
        
        //InputSystem.onDeviceChange += DeviceChange;

        InputUser.onChange+= OnDeviceChanged;


    }

    private void OnDeviceChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if(change.ToString()=="DevicePaired" && device!=null)        
        {
            //ClearConsole.clear();
            //Debug.Log("Change: "+change);
            //Debug.Log("Device: "+device.name);
            _deviceUsing=device.name;
        }
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
        verticalHistory.AddFirst(Vertical);
        horizontalHistory.AddFirst(Horizontal);
        var multiplier=1f;
        if(InputHelper.DeviceInputTool.IsUsingController())
        {
            multiplier=0.5f;            
        }
        while(verticalHistory.Count>historyMaxLength*multiplier)
        {
            verticalHistory.RemoveLast();
            horizontalHistory.RemoveLast();            
        }
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
        float verticalSum=0f;
        float horizontalSum=0f;
        foreach(float number in verticalHistory)
        {
            verticalSum+=Mathf.Abs(number);
        }
        
        foreach(float number in horizontalHistory)
        {
            horizontalSum+=Mathf.Abs(number);
        }
        float verticalAverage=verticalSum/((float)verticalHistory.Count);
        float horizontalAverage=horizontalSum/((float)horizontalHistory.Count);
      
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
