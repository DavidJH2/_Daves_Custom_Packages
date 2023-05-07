using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DHTInput : DHTPlayerInput
{
    // private  DHTPlayerInput _input = new DHTPlayerInput();
    internal InputAction _grabAction;
    internal InputAction _grabbingAction;

    private void OnEnable()
    {
        // Debug.Log("Idle State: Input Enabled");
        Enable();
    }


    private void OnDisable()
    {
        // Debug.Log("Idle State: Input Disabled");
        Disable();
    }

    
    public float GrabValue()
    {
        return InitialActionMap.GrabValue.ReadValue<float>();
    }
}
