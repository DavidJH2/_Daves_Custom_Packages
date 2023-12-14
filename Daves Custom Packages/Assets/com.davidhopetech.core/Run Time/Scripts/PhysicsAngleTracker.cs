using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAngleTracker : MonoBehaviour
{
    [SerializeField] private Transform       target;
    
    private Rigidbody         rb;
    private ConfigurableJoint cj;
    private Vector3           originalLocalPos;
    
    void Start()
    {
        rb          = GetComponent<Rigidbody>();
        originalLocalPos = transform.localPosition;
    }


    void RotateLocalToTarget()
    {
        // transform.localPosition = originalLocalPos;
         rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        // rb.MoveRotation(target.rotation);

        var deltaRot =  target.rotation * Quaternion.Inverse(transform.rotation);

        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
        angle = (angle < 180) ? angle : angle - 360; 
        var axialRot = angle * Mathf.Deg2Rad * axis;

        if (angle != 0)
        {
            var angularVelocity = axialRot / Time.fixedDeltaTime;
            rb.angularVelocity = angularVelocity;
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
        /*
        */
    }

    
    void FixedUpdate()
    {
        RotateLocalToTarget();
    }

    private void LateUpdate()
    {
        transform.localPosition = originalLocalPos;
    }
}
