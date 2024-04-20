using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class MirrorHand : MonoBehaviour
    {
        [FormerlySerializedAs("target")] [SerializeField] public   Transform           xrControler;
        [SerializeField] internal Transform           interactionPoint;
        [SerializeField] internal bool                active = true;
        [SerializeField] private  float               torqueCoeff;
        [SerializeField] private  bool                debug;
        [SerializeField] internal InputActionProperty grabValue;
        [SerializeField] internal InputActionProperty triggerValue;

        internal DTHRingBuffer<Vector3> velocityBuffer;
    
        internal bool    grabStarted;
        internal bool    grabStopped;
        public   bool    triggerPulledThisFrame;
        private  Vector3 lastXRControlerPos;

        private Rigidbody rb;

        protected DHTEventService             dhtEventService;
        protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
        protected DHTUpdateDebugTeleportEvent TeleportEvent;
        protected DHTUpdateDebugValue1Event   DebugValue1Event;

        private DHTDebugPanel_1_Service _dhtDebugPanel_1_Service;
        private DHTLogService              logService;

    
        void Start()
        {
            velocityBuffer = new DTHRingBuffer<Vector3>(5);
            
            lastXRControlerPos = xrControler.position;
            logService         = DHTServiceLocator.Get<DHTLogService>();

            _dhtDebugPanel_1_Service = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
            
            dhtEventService  = DHTServiceLocator.Get<DHTEventService>();

            if (dhtEventService)
            {
                DebugMiscEvent   = dhtEventService.Get<DHTUpdateDebugMiscEvent>();
                TeleportEvent    = dhtEventService.Get<DHTUpdateDebugTeleportEvent>();
                DebugValue1Event = dhtEventService.Get<DHTUpdateDebugValue1Event>();
            }

            rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            SetGrabFlags();
        }


        void FixedUpdate()
        {
            var velocity = (xrControler.position - lastXRControlerPos) / Time.fixedDeltaTime;
            velocityBuffer.Add(velocity);
            lastXRControlerPos = xrControler.position;
            
            if (active)
            {
                MoveHandToTargetOrientation();
            }
        }

        Vector3 lastVelocity
        {
            get
            {
                return velocityBuffer[0];
            }
        }

        void MoveHandToTargetOrientation()
        {
            var vel = (xrControler.position - transform.position) / Time.fixedDeltaTime;
            vel.Clamp(0,1);
            rb.SetVelocty(vel); //
        
            var deltaRot = xrControler.rotation * Quaternion.Inverse(transform.rotation);

            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);


            angle = (angle < 180) ? angle : angle - 360; 
            var axialRot = angle * Mathf.Deg2Rad * axis;

            if (angle != 0)
            {
                // var torque = axialRot * torqueCoeff;
                // rb.AddTorque(torque);
            
                var angularVelocity = axialRot / Time.fixedDeltaTime;
                rb.angularVelocity = angularVelocity;
            }
        }


        public float GrabValue    => grabValue.action.ReadValue<float>();
        public float TriggerValue => triggerValue.action?.ReadValue<float>() ?? 0;
        public bool IsGrabbing
        {
            get
            {
                return GrabValue > .3;
            }
        }

        public bool TriggerPulled => TriggerValue > .3;

		
        private bool _lastIsGrabbing;
        private bool _lastTriggerPulled;
    
        public void SetGrabFlags()
        {
            if (name.Contains("Left"))
            {
                if(_dhtDebugPanel_1_Service) _dhtDebugPanel_1_Service.SetElement(0, $"Left Trigger Pulled: {TriggerPulled}", "");
            }
            else
            {
                if(_dhtDebugPanel_1_Service) _dhtDebugPanel_1_Service.SetElement(1, $"Right Trigger Pulled: {TriggerPulled}", "");
            }

            grabStarted     = (IsGrabbing && !_lastIsGrabbing);
            grabStopped     = (!IsGrabbing && _lastIsGrabbing);
            _lastIsGrabbing = IsGrabbing;

            triggerPulledThisFrame = (TriggerPulled && !_lastTriggerPulled);
            _lastTriggerPulled     = TriggerPulled;
        }
    }
}
