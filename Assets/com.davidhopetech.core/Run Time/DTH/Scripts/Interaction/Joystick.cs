using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField] private GameObject Handle;
        [SerializeField] private Transform  GrabPoint;
        [SerializeField] private float      deadZoneAngle;

        private Vector3 ZeroDirection;
    
        public event Action<float, float> JoyStickEvent = delegate(float f, float f1) {  };
    
        void Start()
        {
        
            ZeroDirection = GrabPoint.position - Handle.transform.position;
        }

    
        void FixedUpdate()
        {
            var up   = Vector3.up;
            var dir  = Handle.transform.localRotation * up;
            var xDir = new Vector3(dir.x, dir.y, 0);
            var zDir = new Vector3(0, dir.y, dir.z);

            var angX = Vector3.SignedAngle(Vector3.up, xDir, Vector3.back);
            var angZ = Vector3.SignedAngle(Vector3.up, zDir, Vector3.right);
            
            // var ax = Vector3.Angle(xDir, up);
            // var az = Vector3.Angle(zDir, up);
            
            JoyStickEvent.Invoke(angX, angZ);
        }
    }
}
