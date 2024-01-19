//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/com.davidhopetech.core/Run Time/Input/DHT Player Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DHTPlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DHTPlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DHT Player Input"",
    ""maps"": [
        {
            ""name"": ""Initial Action Map"",
            ""id"": ""64a3dcd1-ffb8-4dd2-b21d-5cf1a87b079a"",
            ""actions"": [
                {
                    ""name"": ""Grab Value"",
                    ""type"": ""Value"",
                    ""id"": ""9ade250f-332f-41cd-902d-84cd25da7a2e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""6af947fa-aecd-4522-950f-4ff194466d37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""53b96037-ba22-4868-881f-c80b42479d66"",
                    ""path"": ""<XRController>{RightHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grab Value"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad3a9557-8b8e-4d59-afef-453b02d78e97"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Initial Action Map
        m_InitialActionMap = asset.FindActionMap("Initial Action Map", throwIfNotFound: true);
        m_InitialActionMap_GrabValue = m_InitialActionMap.FindAction("Grab Value", throwIfNotFound: true);
        m_InitialActionMap_Grab = m_InitialActionMap.FindAction("Grab", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Initial Action Map
    private readonly InputActionMap m_InitialActionMap;
    private List<IInitialActionMapActions> m_InitialActionMapActionsCallbackInterfaces = new List<IInitialActionMapActions>();
    private readonly InputAction m_InitialActionMap_GrabValue;
    private readonly InputAction m_InitialActionMap_Grab;
    public struct InitialActionMapActions
    {
        private @DHTPlayerInput m_Wrapper;
        public InitialActionMapActions(@DHTPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @GrabValue => m_Wrapper.m_InitialActionMap_GrabValue;
        public InputAction @Grab => m_Wrapper.m_InitialActionMap_Grab;
        public InputActionMap Get() { return m_Wrapper.m_InitialActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InitialActionMapActions set) { return set.Get(); }
        public void AddCallbacks(IInitialActionMapActions instance)
        {
            if (instance == null || m_Wrapper.m_InitialActionMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InitialActionMapActionsCallbackInterfaces.Add(instance);
            @GrabValue.started += instance.OnGrabValue;
            @GrabValue.performed += instance.OnGrabValue;
            @GrabValue.canceled += instance.OnGrabValue;
            @Grab.started += instance.OnGrab;
            @Grab.performed += instance.OnGrab;
            @Grab.canceled += instance.OnGrab;
        }

        private void UnregisterCallbacks(IInitialActionMapActions instance)
        {
            @GrabValue.started -= instance.OnGrabValue;
            @GrabValue.performed -= instance.OnGrabValue;
            @GrabValue.canceled -= instance.OnGrabValue;
            @Grab.started -= instance.OnGrab;
            @Grab.performed -= instance.OnGrab;
            @Grab.canceled -= instance.OnGrab;
        }

        public void RemoveCallbacks(IInitialActionMapActions instance)
        {
            if (m_Wrapper.m_InitialActionMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInitialActionMapActions instance)
        {
            foreach (var item in m_Wrapper.m_InitialActionMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InitialActionMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InitialActionMapActions @InitialActionMap => new InitialActionMapActions(this);
    public interface IInitialActionMapActions
    {
        void OnGrabValue(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
    }
}
