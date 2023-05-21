using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MirrorHand : MonoBehaviour
{
    [SerializeField] internal Transform           target;
    [SerializeField] internal Transform           interactionPoint;
    [SerializeField] internal bool                active = true;
    [SerializeField] private  float               torqueCoeff;
    [SerializeField] private  bool                debug;
    [SerializeField] internal InputActionProperty grabValue;
    
    internal bool grabStarted;
    internal bool grabStopped;

    private   bool                        _lastIsGrabbing;
    private   Rigidbody                   rb;

    protected DHTEventService dhtEventService;
    protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
    protected DHTUpdateDebugTeleportEvent TeleportEvent;
    protected DHTUpdateDebugValue1Event   DebugValue1Event;

    
    void Start()
    {
        dhtEventService  = DHTServiceLocator.dhtEventService;
        DebugMiscEvent   = dhtEventService.dhtUpdateDebugMiscEvent;
        TeleportEvent    = dhtEventService.dhtUpdateDebugTeleportEvent;
        DebugValue1Event = dhtEventService.dhtUpdateDebugValue1Event;
        rb               = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        SetGrabFlags();
    }


    void FixedUpdate()
    {
        if (active)
        {
            MoveHandToTargetOrientation();
        }
    }

    void MoveHandToTargetOrientation()
    {
        var vel = (target.position - transform.position) / Time.fixedDeltaTime;
        vel.Clamp(0,3);
        rb.velocity = vel; 
        
        var deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);

        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);


        angle = (angle < 180) ? angle : angle - 360; 
        var axialRot = angle * Mathf.Deg2Rad * axis;

        if (angle != 0)
        {
            // var torque = axialRot * torqueCoeff;
            // rb.AddTorque(torque);
            
            var angularVelocity = axialRot / Time.fixedDeltaTime;
            rb.angularVelocity = angularVelocity;
        }
    }


    public float GrabValue => grabValue.action.ReadValue<float>();
    internal bool IsGrabbing =>  GrabValue > .1; 

		
    public void SetGrabFlags()
    {
        /*
        if (name == "Right Mirror Hand")
        {
            TeleportEvent.Invoke($"Grab: {GrabValue}");
        }
        */
        grabStarted = (IsGrabbing && !_lastIsGrabbing);
        grabStopped = (!IsGrabbing && _lastIsGrabbing);

        _lastIsGrabbing = IsGrabbing;
    }
}
