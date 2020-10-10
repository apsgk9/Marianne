// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2afa339e-e207-4c97-bf24-30b3f11486c8"",
            ""actions"": [
                {
                    ""name"": ""MovementAxis"",
                    ""type"": ""Value"",
                    ""id"": ""dc041b8d-7752-40cb-b2cf-b4c563f6acb2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""877d28e0-2120-446b-95f1-53c681e76e3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AnalogAim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""55c35701-7188-46f0-adc5-220944249f99"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseAim"",
                    ""type"": ""Value"",
                    ""id"": ""3c3fcbb5-1a08-4fb6-a277-8ea84a93200a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseDeltaAim"",
                    ""type"": ""Value"",
                    ""id"": ""c310d3a3-ba23-4930-a33a-2608fcdab2e4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b9cb2286-d039-4b0f-beba-58e2a980b5ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Recenter"",
                    ""type"": ""Button"",
                    ""id"": ""de48e09b-17f5-46e8-9fec-38ac71a4dd4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Value"",
                    ""id"": ""ad251588-8ab6-4cfa-aa95-7263183e954a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""be29eb5b-65e3-4195-b404-cb1e3763c6ee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""52e79092-ce3f-41f9-8d07-c433a447cb68"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""27efe4b5-931b-4549-95ca-353af563a05e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""31d8f469-5e01-43b1-ab88-e1fc838bf839"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5b9fa201-db36-4ac0-828b-258a64a9aa80"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""325e10f6-b49e-46dc-a97a-64fff9866504"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MovementAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f02416fe-c57b-43b0-b441-a4f25e3f9316"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93759dad-b974-4a49-b176-2820675836b5"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f301fcb-1c5a-46d8-bd93-43c92abb7648"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""453f9506-c5b7-4881-8413-213f3a0db1df"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseDeltaAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cac79751-f183-434e-92d7-be1b871172da"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AnalogAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da78a2ad-da56-4591-ad62-27a501e93dfe"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Recenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ed7ca45-5df5-4edd-aaa8-9e11b9d2aca3"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Recenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb02d518-062e-4ada-8a34-37aa6070887e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43c6d76c-300d-48a9-92a9-e86abfb62e03"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MovementAxis = m_Player.FindAction("MovementAxis", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_AnalogAim = m_Player.FindAction("AnalogAim", throwIfNotFound: true);
        m_Player_MouseAim = m_Player.FindAction("MouseAim", throwIfNotFound: true);
        m_Player_MouseDeltaAim = m_Player.FindAction("MouseDeltaAim", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Recenter = m_Player.FindAction("Recenter", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MovementAxis;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_AnalogAim;
    private readonly InputAction m_Player_MouseAim;
    private readonly InputAction m_Player_MouseDeltaAim;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Recenter;
    private readonly InputAction m_Player_Run;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementAxis => m_Wrapper.m_Player_MovementAxis;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @AnalogAim => m_Wrapper.m_Player_AnalogAim;
        public InputAction @MouseAim => m_Wrapper.m_Player_MouseAim;
        public InputAction @MouseDeltaAim => m_Wrapper.m_Player_MouseDeltaAim;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Recenter => m_Wrapper.m_Player_Recenter;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MovementAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementAxis;
                @MovementAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementAxis;
                @MovementAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementAxis;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @AnalogAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogAim;
                @AnalogAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogAim;
                @AnalogAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogAim;
                @MouseAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseAim;
                @MouseAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseAim;
                @MouseAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseAim;
                @MouseDeltaAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDeltaAim;
                @MouseDeltaAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDeltaAim;
                @MouseDeltaAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDeltaAim;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Recenter.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @Recenter.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @Recenter.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementAxis.started += instance.OnMovementAxis;
                @MovementAxis.performed += instance.OnMovementAxis;
                @MovementAxis.canceled += instance.OnMovementAxis;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @AnalogAim.started += instance.OnAnalogAim;
                @AnalogAim.performed += instance.OnAnalogAim;
                @AnalogAim.canceled += instance.OnAnalogAim;
                @MouseAim.started += instance.OnMouseAim;
                @MouseAim.performed += instance.OnMouseAim;
                @MouseAim.canceled += instance.OnMouseAim;
                @MouseDeltaAim.started += instance.OnMouseDeltaAim;
                @MouseDeltaAim.performed += instance.OnMouseDeltaAim;
                @MouseDeltaAim.canceled += instance.OnMouseDeltaAim;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Recenter.started += instance.OnRecenter;
                @Recenter.performed += instance.OnRecenter;
                @Recenter.canceled += instance.OnRecenter;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovementAxis(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnAnalogAim(InputAction.CallbackContext context);
        void OnMouseAim(InputAction.CallbackContext context);
        void OnMouseDeltaAim(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRecenter(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
}
