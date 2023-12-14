using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DHTButton : MonoBehaviour
{
    public                                   float value;
    [FormerlySerializedAs("pressed")] public bool  isPressed;
    
    [SerializeField]                                      private float min;
    [SerializeField]                                      private float max;
    [SerializeField]                                      private float target;
    [SerializeField]                                      private float spring;
    [SerializeField]                                      private float damp;
    [FormerlySerializedAs("actvateY")] [SerializeField]   private float actvateValue;
    [FormerlySerializedAs("deactvateY")] [SerializeField] private float deactvateValue;

    private Rigidbody _rb;

    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        var pos = transform.localPosition;
        EnforceLimits(ref pos);
        ApplyForces(pos);
        UpdateStates(pos);
        transform.localPosition = pos;
    }

    private void UpdateStates(Vector3 pos)
    {
        float y = pos.y;
        float range = target - min;

        value = (target-y)/range;

        if (isPressed)
        {
            if (value < deactvateValue)
            {
                isPressed = false;
            }
        }
        else
        {
            if (value > actvateValue)
            {
                isPressed = true;
            }
        }
    }

    void EnforceLimits(ref Vector3 pos)
    {
        if (pos.y < min)
        {
            pos.y        = min;
            _rb.velocity = Vector3.zero;
        }

        if (pos.y > max)
        {
            pos.y        = max;
            _rb.velocity = Vector3.zero;
        }
    }


    void ApplyForces(Vector3 pos)
    {
        var springForce = (target - pos.y) * spring;
        var dampeningForce   = _rb.velocity.y * -damp;
        var totalForce  = (springForce + dampeningForce) * transform.up;
        
        _rb.AddForce(totalForce, ForceMode.Force);
    }
}
