using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class DHTHMDService : MonoBehaviour
{
    private bool        HMDFound;
    private InputDevice inputDevice ;


    public UnityEvent<bool> UserPresence;

    private void Start()
    {
        Debug.Log("Start: UserPresence");
    }

    
    private bool lastHmdMounted;
    
    void Update()
    {
        if (HMDFound)
        {
            Vector3 velocity;
            inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
            var hmdMounted =  velocity != Vector3.zero;
            if (hmdMounted != lastHmdMounted)
            {
                UserPresence.Invoke(hmdMounted);
                lastHmdMounted = hmdMounted;
            }
        }
        else
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);

            foreach (var device in inputDevices)
            {
                if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
                {
                    inputDevice = device;
                    HMDFound    = true;
                }
            }
        }
    }

    
}
