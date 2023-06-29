using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ZombieBrain : MonoBehaviour
{
    private ZombieTarget target;
    private Rigidbody    rb;

    private float angY = 0;

    
    void Start()
    {
        target = FindObjectOfType<ZombieTarget>();
        rb     = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateRotation();
    }

    private Vector3 zombiePos;
    private Vector3 targatPos;
    private Vector3 delta;
    private float   ang;
    
    
    private void UpdateRotation()
    {
        zombiePos = transform.position;
        targatPos = target.transform.position;
        delta     = targatPos - zombiePos;
        delta.y = 0;

        ang = Vector3.SignedAngle(rb.transform.forward, delta, Vector3.up);

        
        
        // var deltaRot = Quaternion.Euler(0, ang * .008f, 0);
        // var rot      = rb.rotation;

        angY += ang * Time.fixedDeltaTime * 2f;

        //var newRot = rot * deltaRot;


        rb.rotation        = Quaternion.Euler(0, angY, 0);
        rb.angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            var zombiePos = transform.position;
            var targatPos = target.transform.position;

            // Gizmos.DrawLine(zombiePos, targatPos);

            var message = $"Angle: {(int) ang}\n";
            message += $"Y-Rot: {(int)rb.rotation.eulerAngles.y}";
            // Handles.Label(transform.position, message);
        }
    }
}
