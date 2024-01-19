using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof (XROrigin))]
public class DHTXROrigin : MonoBehaviour
{
    [SerializeField] private XROrigin xrOrigin;
    
    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = GetComponent<XROrigin>();
        }
    }


    void Update()
    {
        
    }
    
    
    
    public void SetVRMode(bool vrSittingMode)
    {
        if (vrSittingMode)
        {
            xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        }
        else
        {
            xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        }
    }
}
