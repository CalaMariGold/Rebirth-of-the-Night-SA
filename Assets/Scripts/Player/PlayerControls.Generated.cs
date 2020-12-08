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
            ""name"": ""Default"",
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
                },
                {
                    ""name"": ""Raise Terrain"",
                    ""type"": ""Button"",
                    ""id"": ""c2521131-8147-4c5c-8fc5-c1dd4726c60e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dig Terrain"",
                    ""type"": ""Button"",
                    ""id"": ""32bc64bd-2c2f-41f8-9fa6-4ac212f6b74b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Smooth Terrain"",
                    ""type"": ""Button"",
                    ""id"": ""4475472b-dc72-4440-9fa0-54a9bfe5572e"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""6873c12b-87c5-4dcc-8414-6be0ccda8485"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Raise Terrain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ebea706-2456-4d5f-bd36-fae95ec0cd17"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dig Terrain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d6592ae-9c1a-4584-a97f-b41731be06cb"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Smooth Terrain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Default
            m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
            m_Default_Look = m_Default.FindAction("Look", throwIfNotFound: true);
            m_Default_Move = m_Default.FindAction("Move", throwIfNotFound: true);
            m_Default_Crouch = m_Default.FindAction("Crouch", throwIfNotFound: true);
            m_Default_Sprint = m_Default.FindAction("Sprint", throwIfNotFound: true);
            m_Default_Jump = m_Default.FindAction("Jump", throwIfNotFound: true);
            m_Default_RaiseTerrain = m_Default.FindAction("Raise Terrain", throwIfNotFound: true);
            m_Default_DigTerrain = m_Default.FindAction("Dig Terrain", throwIfNotFound: true);
            m_Default_SmoothTerrain = m_Default.FindAction("Smooth Terrain", throwIfNotFound: true);
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

        // Default
        private readonly InputActionMap m_Default;
        private IDefaultActions m_DefaultActionsCallbackInterface;
        private readonly InputAction m_Default_Look;
        private readonly InputAction m_Default_Move;
        private readonly InputAction m_Default_Crouch;
        private readonly InputAction m_Default_Sprint;
        private readonly InputAction m_Default_Jump;
        private readonly InputAction m_Default_RaiseTerrain;
        private readonly InputAction m_Default_DigTerrain;
        private readonly InputAction m_Default_SmoothTerrain;
        public struct DefaultActions
        {
            private @PlayerControls m_Wrapper;
            public DefaultActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Look => m_Wrapper.m_Default_Look;
            public InputAction @Move => m_Wrapper.m_Default_Move;
            public InputAction @Crouch => m_Wrapper.m_Default_Crouch;
            public InputAction @Sprint => m_Wrapper.m_Default_Sprint;
            public InputAction @Jump => m_Wrapper.m_Default_Jump;
            public InputAction @RaiseTerrain => m_Wrapper.m_Default_RaiseTerrain;
            public InputAction @DigTerrain => m_Wrapper.m_Default_DigTerrain;
            public InputAction @SmoothTerrain => m_Wrapper.m_Default_SmoothTerrain;
            public InputActionMap Get() { return m_Wrapper.m_Default; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
            public void SetCallbacks(IDefaultActions instance)
            {
                if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
                {
                    @Look.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                    @Move.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                    @Crouch.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCrouch;
                    @Crouch.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCrouch;
                    @Crouch.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCrouch;
                    @Sprint.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSprint;
                    @Sprint.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSprint;
                    @Sprint.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSprint;
                    @Jump.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                    @RaiseTerrain.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaiseTerrain;
                    @RaiseTerrain.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaiseTerrain;
                    @RaiseTerrain.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaiseTerrain;
                    @DigTerrain.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDigTerrain;
                    @DigTerrain.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDigTerrain;
                    @DigTerrain.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDigTerrain;
                    @SmoothTerrain.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSmoothTerrain;
                    @SmoothTerrain.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSmoothTerrain;
                    @SmoothTerrain.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSmoothTerrain;
                }
                m_Wrapper.m_DefaultActionsCallbackInterface = instance;
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
                    @RaiseTerrain.started += instance.OnRaiseTerrain;
                    @RaiseTerrain.performed += instance.OnRaiseTerrain;
                    @RaiseTerrain.canceled += instance.OnRaiseTerrain;
                    @DigTerrain.started += instance.OnDigTerrain;
                    @DigTerrain.performed += instance.OnDigTerrain;
                    @DigTerrain.canceled += instance.OnDigTerrain;
                    @SmoothTerrain.started += instance.OnSmoothTerrain;
                    @SmoothTerrain.performed += instance.OnSmoothTerrain;
                    @SmoothTerrain.canceled += instance.OnSmoothTerrain;
                }
            }
        }
        public DefaultActions @Default => new DefaultActions(this);
        public interface IDefaultActions
        {
            void OnLook(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnRaiseTerrain(InputAction.CallbackContext context);
            void OnDigTerrain(InputAction.CallbackContext context);
            void OnSmoothTerrain(InputAction.CallbackContext context);
        }
    }
}
