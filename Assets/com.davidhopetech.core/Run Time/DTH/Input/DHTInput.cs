using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DHTInput : DHTPlayerInput
{
    // private  DHTPlayerInput _input = new DHTPlayerInput();
    internal InputAction _grabAction;
    internal InputAction _grabbingAction;
    internal bool        _isGrabing;
    internal float       _grabValue;

    private void OnEnable()
    {
        // Debug.Log("Idle State: Input Enabled");
        Enable();
        _grabbingAction.started  += StartedGrabbing;
        _grabbingAction.canceled += StopedGrabbing;
    }


    private void OnDisable()
    {
        // Debug.Log("Idle State: Input Disabled");
        _grabbingAction.started  -= StartedGrabbing;
        _grabbingAction.canceled -= StopedGrabbing;
        Disable();
    }

	
    private void StartedGrabbing(InputAction.CallbackContext obj)
    {
        // Debug.Log("Started Grabbing");
        _isGrabing = true;
    }

    private void StopedGrabbing(InputAction.CallbackContext obj)
    {
        // Debug.Log("Stopped Grabbing");
        _isGrabing = false;
    }

    public float GrabValue()
    {
        return InitialActionMap.GrabValue.ReadValue<float>();
    }
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
