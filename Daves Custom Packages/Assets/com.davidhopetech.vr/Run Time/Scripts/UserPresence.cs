using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class UserPresence : MonoBehaviour
{
    [SerializeField] private GameObject vrCam;
    [SerializeField] private GameObject cam;
    
    public void OnUserPresence(bool hmdMounted)
    {
#if UNITY_STANDALONE_WIN
        if (hmdMounted)
        {
            Debug.Log("User Presence");
            vrCam.SetActive(true);
            cam.SetActive(false);
        }
        else
        {
            Debug.Log("No User Presence");
            vrCam.SetActive(false);
            cam.SetActive(true);
        }
#endif
#if PLATFORM_ANDROID
        vrCam.SetActive(true);
        cam.SetActive(false);
#endif
    }
}
