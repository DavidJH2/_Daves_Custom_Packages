using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class HandActuator : MonoBehaviour
    {
        public InputActionProperty pinchAnimationAction;
        public InputActionProperty gripAnimationAction;
        public Animator            handAnimator;

        protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
        protected DHTUpdateDebugTeleportEvent TeleportEvent;
        protected DHTUpdateDebugValue1Event   DebugValue1Event;
        protected DHTEventService             EventService ;

        private void Awake()
        {
            EventService = DHTServiceLocator.Get<DHTEventService>();

            if (EventService)
            {
                DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
                TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
                DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
            }
        }

        
        public float gripValue    => gripAnimationAction.action.ReadValue<float>();
        public float triggerValue => pinchAnimationAction.action.ReadValue<float>();

        
        void Update()
        {
            handAnimator.SetFloat("Grip", gripValue);           // Todo: change to Index lookup
            handAnimator.SetFloat("Trigger", triggerValue);
        }
    }
}