using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

public class DHTHMDService : MonoBehaviour
{
    public UnityEvent<bool> UserPresence = new UnityEvent<bool>();

    private InputDevice inputDevice ;
    private Action      state;
    private bool        lastHmdMounted;

    
    void Start()
    {
        SetState(FindHMD);
    }
    
    
    void SetState(Action newState)
    {
        state = newState;
    }

    
    void Update()
    {
        state.Invoke();
    }

    void FindHMD()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
            {
                inputDevice = device;
                SetState(UpdateHMDUserPresence);
            }
        }
    }

    void UpdateHMDUserPresence()
    {
        Vector3 velocity;
        inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
        var hmdMounted =  (velocity != Vector3.zero);
        if (hmdMounted != lastHmdMounted)
        {
            UserPresence.Invoke(hmdMounted);
            lastHmdMounted = hmdMounted;
        }
    }

    /*
    private void Start()
    {
        // Subscribe to the session state change event
        OpenXRRuntime.wantsToQuit    += OnSessionEnding;
        OpenXRRuntime.wantsToRestart += OnSessionRestarting;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the session state change event
        OpenXRRuntime.wantsToQuit    -= OnSessionEnding;
        OpenXRRuntime.wantsToRestart -= OnSessionRestarting;
    }

    private bool OnSessionEnding()
    {
        UserPresence.Invoke(false);     // HMD removed
        
        Debug.Log("OpenXR session is ending");
        // Return true to allow the session to end, or false to prevent it
        return true;
    }

    private bool OnSessionRestarting()
    {
        UserPresence.Invoke(true);      // HMD put on
        
        Debug.Log("OpenXR session is restarting");
        // Return true to allow the session to restart, or false to prevent it
        return true;
    }    
    */
}
