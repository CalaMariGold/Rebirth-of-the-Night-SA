// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Rebirth.Player
{
    public class @PlayerControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""bd370564-71de-450d-bffe-03e323faccdf"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""50445499-dbe4-4ccb-bc63-5400cecc3ab7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""67a52e70-3f6a-4cfa-b2e2-e59bae22358e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""e19273af-181c-435b-b114-da6381945040"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""bfb495a8-44e2-4431-8c6e-d04c3140731c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""24b58a6b-d6d1-4879-acfb-faa998456d46"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3f45fbac-bf8e-4a2c-8b68-470ebba29272"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""303ca056-49f5-4bb0-8ef3-1c3ccaf27f8d"",
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
                    ""id"": ""96473374-2a75-403c-b513-c23607aabf52"",
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
                    ""id"": ""0b48c349-6d97-47e7-98ea-60ab1c771943"",
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
                    ""id"": ""7f2a3221-c822-4d59-a228-19fbeedc88c7"",
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
                    ""id"": ""fd79200a-3908-4284-b21c-2bdc0bae04d1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""af840b76-eb02-4a63-b98d-d5a2a2872c59"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d11a3a00-5a72-4507-91a2-4cb94a2b4921"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4801f15-0952-43bb-bc56-30a2334d5c6c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""e854ae95-49ea-4939-940d-af17e7b5750a"",
            ""actions"": [
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""59ce15db-6b7a-486d-a305-7828e8a2db78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aa84f9cd-3da6-4b37-9f82-d2fafc9a2d99"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
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
            m_Movement_Look = m_Movement.FindAction("Look", throwIfNotFound: true);
            m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
            m_Movement_Crouch = m_Movement.FindAction("Crouch", throwIfNotFound: true);
            m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
            m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Inventory = m_UI.FindAction("Inventory", throwIfNotFound: true);
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
        private readonly InputAction m_Movement_Look;
        private readonly InputAction m_Movement_Move;
        private readonly InputAction m_Movement_Crouch;
        private readonly InputAction m_Movement_Sprint;
        private readonly InputAction m_Movement_Jump;
        public struct MovementActions
        {
            private @PlayerControls m_Wrapper;
            public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Look => m_Wrapper.m_Movement_Look;
            public InputAction @Move => m_Wrapper.m_Movement_Move;
            public InputAction @Crouch => m_Wrapper.m_Movement_Crouch;
            public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
            public InputAction @Jump => m_Wrapper.m_Movement_Jump;
            public InputActionMap Get() { return m_Wrapper.m_Movement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
            public void SetCallbacks(IMovementActions instance)
            {
                if (m_Wrapper.m_MovementActionsCallbackInterface != null)
                {
                    @Look.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                    @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                    @Crouch.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                    @Crouch.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                    @Crouch.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                    @Sprint.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                    @Sprint.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                    @Sprint.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                    @Jump.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                }
                m_Wrapper.m_MovementActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Crouch.started += instance.OnCrouch;
                    @Crouch.performed += instance.OnCrouch;
                    @Crouch.canceled += instance.OnCrouch;
                    @Sprint.started += instance.OnSprint;
                    @Sprint.performed += instance.OnSprint;
                    @Sprint.canceled += instance.OnSprint;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                }
            }
        }
        public MovementActions @Movement => new MovementActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_Inventory;
        public struct UIActions
        {
            private @PlayerControls m_Wrapper;
            public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Inventory => m_Wrapper.m_UI_Inventory;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @Inventory.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                    @Inventory.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                    @Inventory.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Inventory.started += instance.OnInventory;
                    @Inventory.performed += instance.OnInventory;
                    @Inventory.canceled += instance.OnInventory;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface IMovementActions
        {
            void OnLook(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnInventory(InputAction.CallbackContext context);
        }
    }
}
