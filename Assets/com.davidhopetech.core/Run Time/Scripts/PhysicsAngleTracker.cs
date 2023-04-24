using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAngleTracker : MonoBehaviour
{
    [SerializeField] private Transform       target;
    
    private Rigidbody       rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void RotateLocalToTarget()
    {
        var deltaRot =  target.rotation * Quaternion.Inverse(transform.rotation);

        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
        angle = (angle < 180) ? angle : angle - 360; 
        var axialRot = angle * Mathf.Deg2Rad * axis;

        if (angle != 0)
        {
            var angularVelocity = axialRot / Time.fixedDeltaTime;
            rb.angularVelocity = angularVelocity;
        }
    }

    
    void FixedUpdate()
    {
        RotateLocalToTarget();
    }
}
