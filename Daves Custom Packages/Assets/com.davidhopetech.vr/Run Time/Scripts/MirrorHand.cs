using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class MirrorHand : MonoBehaviour
    {
        [SerializeField] public   Transform           target;
        [SerializeField] internal Transform           interactionPoint;
        [SerializeField] internal bool                active = true;
        [SerializeField] private  float               torqueCoeff;
        [SerializeField] private  bool                debug;
        [SerializeField] internal InputActionProperty grabValue;
        [SerializeField] internal InputActionProperty triggerValue;
    
        internal bool grabStarted;
        internal bool grabStopped;
        public   bool triggerPulledThisFrame;

        private Rigidbody rb;

        protected DHTEventService             dhtEventService;
        protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
        protected DHTUpdateDebugTeleportEvent TeleportEvent;
        protected DHTUpdateDebugValue1Event   DebugValue1Event;

        private DhtDhtDebugPanel_1_Service _dhtDebugPanel_1_Service;
        private DHTLogService              logService;

    
        void Start()
        {
            logService = DHTServiceLocator.Get<DHTLogService>();

             //_dhtDebugPanel_1_Service = FindObjectOfType<DHTDebugPanel_Service>(true);
            _dhtDebugPanel_1_Service = DHTServiceLocator.Get<DhtDhtDebugPanel_1_Service>();
            
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
            if (active)
            {
                MoveHandToTargetOrientation();
            }
        }

        void MoveHandToTargetOrientation()
        {
            var vel = (target.position - transform.position) / Time.fixedDeltaTime;
            vel.Clamp(0,1);
            rb.SetVelocty(vel); //
        
            var deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);

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
