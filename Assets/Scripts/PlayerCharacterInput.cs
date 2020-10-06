using System;
using System.Collections.Generic;
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

    private LinkedList<float> verticalHistory= new LinkedList<float>();
    private LinkedList<float> horizontalHistory= new LinkedList<float>();

    private const int historyLength=8;

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
        _inputActions.Player.MovementAxis.canceled+= ctx=>HandleMovementCancel();

        _inputActions.Player.MouseAim.performed+= HandleMouseAim;
        _inputActions.Player.MouseDeltaAim.performed+= HandleMouseDeltaAim;
        _inputActions.Player.MouseDeltaAim.canceled+= ctx=>_cursorDeltaPosition= Vector2.zero;

        _inputActions.Player.AnalogAim.performed+= HandleAnalogAim;
        _inputActions.Player.Run.started+=HandleRunPressed;
        _inputActions.Player.Run.canceled+=HandleRunReleased;
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

        MovementHistory();
    }

    private void MovementHistory()
    {
        verticalHistory.AddFirst(Vertical);
        horizontalHistory.AddFirst(Horizontal);
        if(verticalHistory.Count>historyLength)
        {
            verticalHistory.RemoveLast();
            horizontalHistory.RemoveLast();            
        }
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
        //Debug.Log("verticalAverage: "+verticalAverage);
        //Debug.Log("horizontalAverage: "+horizontalAverage);
      
        bool isMovementhere= verticalAverage > Mathf.Epsilon || horizontalAverage > Mathf.Epsilon;
        //bool isMovementhere= Vertical != _previousVertical || _previousHorizontal != Horizontal;
        //bool isAxisCenter= Mathf.Abs(Vertical) < Mathf.Epsilon || Mathf.Abs(Horizontal) < Mathf.Epsilon;
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
}
