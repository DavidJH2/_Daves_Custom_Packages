using System;
using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static com.davidhopetech.core.Run_Time.DTH.ServiceLocator.DHTServiceLocator;

public class SpaceShip : MonoBehaviour
{
	[SerializeField] private float    _turnThreshold;
	[SerializeField] private float    _turnRate;
    [SerializeField] private Joystick _joystick;
    
    private Rigidbody rb;
    private float     turnRate;

    void Start()
    {
	    rb                      =  GetComponent<Rigidbody>();
	    _joystick.JoyStickEvent += OnJoystick;
    }

    private void OnJoystick(float arg1, float arg2)
    {
	    turnRate = 0.0f;
	    
	    if (arg1 > _turnThreshold) turnRate  = -_turnRate;
	    if (arg1 < -_turnThreshold) turnRate = _turnRate;

	    DhtEventService.dhtUpdateDebugTeleportEvent.Invoke($"turn rate: {turnRate}");
    }

    private void FixedUpdate()
    {
	    rb.angularVelocity = turnRate * Mathf.Deg2Rad * Vector3.forward;
    }
}
