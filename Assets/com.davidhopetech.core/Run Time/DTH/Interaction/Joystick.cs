using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private GameObject Handle;
    [SerializeField] private Transform GrabPoint;
    [SerializeField] private float      deadZoneAngle;

    private Vector3 ZeroDirection;
    
    public static event EventHandler<JoystickEventArgs> JoyStickEvent;
    public class JoystickEventArgs : EventArgs
    {
        public float AngX;
        public float AngY;
        
        public JoystickEventArgs(float i_angX, float i_angZ)
        {
            AngX = i_angX;
            AngY = i_angZ;
        }

    }
    
    void Start()
    {
        
        ZeroDirection = GrabPoint.position - Handle.transform.position;
    }

    
    void Update()
    {
        Vector3 newDir = GrabPoint.position - Handle.transform.position;
        var ang = Vector3.Angle(ZeroDirection, newDir);

        if (ang < deadZoneAngle) return;
        JoyStickEvent.Invoke(this, new JoystickEventArgs(ang, 0));
    }
}
