using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction
{
    public class DHTJoystick : MonoBehaviour
    {
        [SerializeField] private GameObject Handle;
        
        public event Action<float, float> JoyStickEvent = delegate(float f, float f1) {  };      // Todo:  === Change from events to polling ===

    
        void FixedUpdate()
        {
            var up   = Vector3.up;
            var dir  = Handle.transform.localRotation * up;
            var xDir = new Vector3(dir.x, dir.y, 0);
            var zDir = new Vector3(0, dir.y, dir.z);

            var angX = Vector3.SignedAngle(Vector3.up, xDir, Vector3.back);
            var angZ = Vector3.SignedAngle(Vector3.up, zDir, Vector3.right);
            
            JoyStickEvent.Invoke(angX, angZ);
        }
    }
}
