using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandPhysics : MonoBehaviour
{
    [SerializeField] private float     torqueCoeff;
    [SerializeField] private bool      debug;
    [SerializeField] private Transform target;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GetBones(firstSourceBone);
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

            /*
            if (debug)
            {
                Debug.Log($"Torque: {torque}");
            }
            */
        }
    }

    
    void FixedUpdate()
    {
        MoveHandToTargetOrientation();
    }
}
