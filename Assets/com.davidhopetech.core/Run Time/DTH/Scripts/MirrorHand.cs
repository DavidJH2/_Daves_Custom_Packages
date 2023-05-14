using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MirrorHand : MonoBehaviour
{
    public bool grabStarted;
    public bool grabStopped;

    [SerializeField] internal InputActionProperty grabValue;

    [SerializeField] private  float     torqueCoeff;
    [SerializeField] private  bool      debug;
    [SerializeField] internal Transform target;
    
    private bool _lastIsGrabbing;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GetBones(firstSourceBone);
    }


    private void Update()
    {
        setGrabFlags();
    }

    void MoveHandToTargetOrientation()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        
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
    internal bool IsGrabbing =>  GrabValue > .01; 

		
    public void setGrabFlags()
    {
        grabStarted = (IsGrabbing && !_lastIsGrabbing);
        grabStopped = (!IsGrabbing && _lastIsGrabbing);

        _lastIsGrabbing = IsGrabbing;
    }
    


    
    void FixedUpdate()
    {
        MoveHandToTargetOrientation();
    }
}
