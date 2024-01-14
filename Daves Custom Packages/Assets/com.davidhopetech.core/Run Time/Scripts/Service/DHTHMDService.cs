using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class DHTHMDService : MonoBehaviour
{
    public  UnityEvent<bool> UserPresence;

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
}
