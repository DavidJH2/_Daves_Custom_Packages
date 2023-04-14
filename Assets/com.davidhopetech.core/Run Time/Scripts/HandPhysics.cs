using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysics : MonoBehaviour
{
    [SerializeField] private Transform target;
    private                  Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        
        var deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);

        target.rotation.ToAngleAxis(out float angle0, out Vector3 axis0);
        transform.rotation.ToAngleAxis(out float angle1, out Vector3 axis1);
        Debug.Log($"Ang0: {angle0*axis0}\t\tAng1: {angle1*axis1}");

        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
        var axialRot = angle * Mathf.Deg2Rad * axis;

        rb.angularVelocity = axialRot / Time.fixedDeltaTime;
        //rb.angularVelocity = Vector3.zero;
        var angularVelocity = axialRot / Time.fixedDeltaTime;
        // Debug.Log($"Rot: {axialRot}\t\tAngVel: {angularVelocity}");
    }
}
