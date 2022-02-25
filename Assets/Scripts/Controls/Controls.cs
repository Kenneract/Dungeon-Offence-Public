// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""8ff7f23e-6e0a-4d74-aeb9-759400a56c83"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f170b5d8-c65a-4fca-bd8a-c0855af27954"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Movement"",
                    ""id"": ""42b522b5-a30e-4f2f-bf5a-7bd07848515a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb7298a9-dfac-4a0c-b5d8-c945ea677cf6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""63289422-ad35-44fc-915f-e4e8bc18b53d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7bc183e8-8c6a-4edd-8eef-41860c0d8aae"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""969ac195-1fff-452c-bfdb-40395c91274d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Interact"",
            ""id"": ""8a5e0748-e8e8-4b24-86c3-8a743689d2ba"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""b445c684-b2d0-49dc-935d-59bed31153eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""414a5423-b0d5-42a1-a83f-f094c6c582c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""098a9cb2-5674-4af3-b33f-9fb3e3a2d29c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d1f09e4-d6c7-48e6-bb3c-6d522fc6c1e0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95489afe-eccc-4fb2-9d96-913cecc87b31"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4c83deb-8949-4e9e-89e6-13d0b6c841b4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""49a58f6b-da34-4061-86d8-0c328f76a78c"",
            ""actions"": [
                {
                    ""name"": ""Toggle"",
                    ""type"": ""Button"",
                    ""id"": ""2081417b-ed54-4a97-b66e-37b851b0beef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""188e5497-3579-4750-841d-c12d1f288099"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3aa96b5c-d46e-4971-bd5b-a96fe4c484c6"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Hotbar"",
            ""id"": ""1d266001-ceb8-4728-8419-c4db8b79b9e6"",
            ""actions"": [
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""051ac27c-3299-4654-9c80-a98ae1349d2f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot1"",
                    ""type"": ""Button"",
                    ""id"": ""009f114b-58be-4015-ad27-150682363afd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot2"",
                    ""type"": ""Button"",
                    ""id"": ""20388ccf-51cf-4956-9aa4-526da359e3e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot3"",
                    ""type"": ""Button"",
                    ""id"": ""c5d5ac90-c7f9-4906-8a0c-dd6ce3ac1e02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot4"",
                    ""type"": ""Button"",
                    ""id"": ""4448410f-a4ba-48f4-a150-be33f76d200a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6fb1d24c-36f5-4482-b106-d3c4bc0d1f90"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e258ccb1-c01c-441d-841e-a8462929f35b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e39d7042-9432-4d6f-8845-0707d65e5c50"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a787b80-deb2-4a89-bb32-fa7222512e16"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f1b3f26-bbef-4582-8da0-25a412216731"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""356e11b3-e149-4d82-98c7-c0a0c193d929"",
            ""actions"": [
                {
                    ""name"": ""Close"",
                    ""type"": ""Button"",
                    ""id"": ""a68a5ebe-10d6-44d7-8a55-1a6cd055e7b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f75bfc92-b067-446a-8532-623513517b29"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Close"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        // Interact
        m_Interact = asset.FindActionMap("Interact", throwIfNotFound: true);
        m_Interact_Attack = m_Interact.FindAction("Attack", throwIfNotFound: true);
        m_Interact_Action = m_Interact.FindAction("Action", throwIfNotFound: true);
        m_Interact_Aim = m_Interact.FindAction("Aim", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_Toggle = m_Inventory.FindAction("Toggle", throwIfNotFound: true);
        // Hotbar
        m_Hotbar = asset.FindActionMap("Hotbar", throwIfNotFound: true);
        m_Hotbar_Scroll = m_Hotbar.FindAction("Scroll", throwIfNotFound: true);
        m_Hotbar_Slot1 = m_Hotbar.FindAction("Slot1", throwIfNotFound: true);
        m_Hotbar_Slot2 = m_Hotbar.FindAction("Slot2", throwIfNotFound: true);
        m_Hotbar_Slot3 = m_Hotbar.FindAction("Slot3", throwIfNotFound: true);
        m_Hotbar_Slot4 = m_Hotbar.FindAction("Slot4", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Close = m_Menu.FindAction("Close", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    public struct MovementActions
    {
        private @Controls m_Wrapper;
        public MovementActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Interact
    private readonly InputActionMap m_Interact;
    private IInteractActions m_InteractActionsCallbackInterface;
    private readonly InputAction m_Interact_Attack;
    private readonly InputAction m_Interact_Action;
    private readonly InputAction m_Interact_Aim;
    public struct InteractActions
    {
        private @Controls m_Wrapper;
        public InteractActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Interact_Attack;
        public InputAction @Action => m_Wrapper.m_Interact_Action;
        public InputAction @Aim => m_Wrapper.m_Interact_Aim;
        public InputActionMap Get() { return m_Wrapper.m_Interact; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractActions set) { return set.Get(); }
        public void SetCallbacks(IInteractActions instance)
        {
            if (m_Wrapper.m_InteractActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnAttack;
                @Action.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnAction;
                @Aim.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m_InteractActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public InteractActions @Interact => new InteractActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_Toggle;
    public struct InventoryActions
    {
        private @Controls m_Wrapper;
        public InventoryActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Toggle => m_Wrapper.m_Inventory_Toggle;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @Toggle.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggle;
                @Toggle.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggle;
                @Toggle.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggle;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Toggle.started += instance.OnToggle;
                @Toggle.performed += instance.OnToggle;
                @Toggle.canceled += instance.OnToggle;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);

    // Hotbar
    private readonly InputActionMap m_Hotbar;
    private IHotbarActions m_HotbarActionsCallbackInterface;
    private readonly InputAction m_Hotbar_Scroll;
    private readonly InputAction m_Hotbar_Slot1;
    private readonly InputAction m_Hotbar_Slot2;
    private readonly InputAction m_Hotbar_Slot3;
    private readonly InputAction m_Hotbar_Slot4;
    public struct HotbarActions
    {
        private @Controls m_Wrapper;
        public HotbarActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Scroll => m_Wrapper.m_Hotbar_Scroll;
        public InputAction @Slot1 => m_Wrapper.m_Hotbar_Slot1;
        public InputAction @Slot2 => m_Wrapper.m_Hotbar_Slot2;
        public InputAction @Slot3 => m_Wrapper.m_Hotbar_Slot3;
        public InputAction @Slot4 => m_Wrapper.m_Hotbar_Slot4;
        public InputActionMap Get() { return m_Wrapper.m_Hotbar; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HotbarActions set) { return set.Get(); }
        public void SetCallbacks(IHotbarActions instance)
        {
            if (m_Wrapper.m_HotbarActionsCallbackInterface != null)
            {
                @Scroll.started -= m_Wrapper.m_HotbarActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_HotbarActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_HotbarActionsCallbackInterface.OnScroll;
                @Slot1.started -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot1;
                @Slot1.performed -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot1;
                @Slot1.canceled -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot1;
                @Slot2.started -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot2;
                @Slot2.performed -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot2;
                @Slot2.canceled -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot2;
                @Slot3.started -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot3;
                @Slot3.performed -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot3;
                @Slot3.canceled -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot3;
                @Slot4.started -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot4;
                @Slot4.performed -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot4;
                @Slot4.canceled -= m_Wrapper.m_HotbarActionsCallbackInterface.OnSlot4;
            }
            m_Wrapper.m_HotbarActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @Slot1.started += instance.OnSlot1;
                @Slot1.performed += instance.OnSlot1;
                @Slot1.canceled += instance.OnSlot1;
                @Slot2.started += instance.OnSlot2;
                @Slot2.performed += instance.OnSlot2;
                @Slot2.canceled += instance.OnSlot2;
                @Slot3.started += instance.OnSlot3;
                @Slot3.performed += instance.OnSlot3;
                @Slot3.canceled += instance.OnSlot3;
                @Slot4.started += instance.OnSlot4;
                @Slot4.performed += instance.OnSlot4;
                @Slot4.canceled += instance.OnSlot4;
            }
        }
    }
    public HotbarActions @Hotbar => new HotbarActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Close;
    public struct MenuActions
    {
        private @Controls m_Wrapper;
        public MenuActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Close => m_Wrapper.m_Menu_Close;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Close.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnClose;
                @Close.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnClose;
                @Close.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnClose;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Close.started += instance.OnClose;
                @Close.performed += instance.OnClose;
                @Close.canceled += instance.OnClose;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IInteractActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnToggle(InputAction.CallbackContext context);
    }
    public interface IHotbarActions
    {
        void OnScroll(InputAction.CallbackContext context);
        void OnSlot1(InputAction.CallbackContext context);
        void OnSlot2(InputAction.CallbackContext context);
        void OnSlot3(InputAction.CallbackContext context);
        void OnSlot4(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnClose(InputAction.CallbackContext context);
    }
}
