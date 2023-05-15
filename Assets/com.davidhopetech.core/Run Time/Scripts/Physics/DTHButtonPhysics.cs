using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DTHButtonPhysics : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private float target;
    [SerializeField] private float spring;
    [SerializeField] private float damp;

    private Rigidbody _rb;
    //private GameObject _buttonBase;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var pos = transform.localPosition;

        if (pos.y < min)
        {
            pos.y = min;
            _rb.velocity = Vector3.zero;
        }

        if (pos.y > max)
        {
            pos.y        = max;
            _rb.velocity = Vector3.zero;
        }

        var springForce = (target - pos.y) * spring;
        var dampeningForce   = _rb.velocity.y * -damp;
        var totalForce  = (springForce + dampeningForce) * transform.up;
        
        _rb.AddForce(totalForce, ForceMode.Force);
        
        transform.localPosition = pos;
    }
}
