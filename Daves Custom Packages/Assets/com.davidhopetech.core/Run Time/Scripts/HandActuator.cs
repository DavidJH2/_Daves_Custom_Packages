using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DTH.Scripts
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
            EventService     = DHTServiceLocator.dhtEventService;
            
            DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
            TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
            DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
        }


        void Update()
        {
            float gripValue = gripAnimationAction.action.ReadValue<float>();
            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);           // Todo: change to Index lookup
            handAnimator.SetFloat("Trigger", triggerValue);
        }
    }
}