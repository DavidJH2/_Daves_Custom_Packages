using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandPhysicsAB : MonoBehaviour
{
    [SerializeField] private float     torqueCoeff;
    [SerializeField] private bool      debug;
    [SerializeField] private Transform target;
    
    private ArticulationBody ab;
    /*
    [SerializeField] private Transform       firstSourceBone;
    
    private List<Transform> SrcBones = new List<Transform>();
    private List<Transform> DestBones = new List<Transform>();
    */
    
    void Start()
    {
        ab = GetComponent<ArticulationBody>();
        //GetBones(firstSourceBone);
    }

    
    /*
    Transform FindGameObjectByName(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        
        var soCount = parent.childCount;
        for (var i = 0; i < soCount; i++)
        {
            var subObj = parent.GetChild(i);
            if (subObj.name == name) return subObj;

            subObj = FindGameObjectByName(subObj, name);
            if (subObj != null) return subObj;
        }

        return null;
    }

    private void GetBones(Transform sourceBone)
    {
        SrcBones.Add(sourceBone);

        var destBone = FindGameObjectByName(transform, sourceBone.name);
        DestBones.Add(destBone);
        
        var numBones = sourceBone.childCount;
        for(var i = 0;i<numBones; i++)
        {
            var child = sourceBone.GetChild(i);
            GetBones(child);
        }
    }
    */

    void MoveHandToTargetOrientation()
    {
        ab.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        
        var deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);

        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);


        angle = (angle < 180) ? angle : angle - 360; 
        var axialRot = angle * Mathf.Deg2Rad * axis;

        if (angle != 0)
        {
            var torque = axialRot * torqueCoeff;
            // ab.AddTorque(torque);
            
            var angularVelocity = axialRot / Time.fixedDeltaTime;
            ab.angularVelocity = angularVelocity;

            if (debug)
            {
                Debug.Log($"Torque: {torque}");
            }
        }
    }


    /*
    void RotateFingersToTargetRotation()
    {
        var count = SrcBones.Count;
        for(var i=0; i<count; i++)
        {
            var srcBone = SrcBones[i];
            var destBone = DestBones[i];

            ab.velocity = (srcBone.position - destBone.position) / Time.fixedDeltaTime;
        
            var deltaRot = srcBone.rotation * Quaternion.Inverse(destBone.rotation);

            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            angle = (angle < 180) ? angle : angle - 360;
            var axialRot = angle * Mathf.Deg2Rad * axis;

            if (angle != 0)
            {
                var angularVelocity = axialRot / Time.fixedDeltaTime;
                ab.angularVelocity = angularVelocity;
            }
        }
    }
    */

    void FixedUpdate()
    {
        MoveHandToTargetOrientation();
        // RotateFingersToTargetRotation();
    }
}
