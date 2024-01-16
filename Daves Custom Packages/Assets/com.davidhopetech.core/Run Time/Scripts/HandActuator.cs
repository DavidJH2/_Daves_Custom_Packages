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

        private DebugPanel debugPanel;
        private void Awake()
        {
            debugPanel = DHTServiceLocator.Instance.Get<DHTDebugPanelService>().debugPanel1;

            // EventService = DHTServiceLocator.dhtEventService;
            EventService = DHTServiceLocator.Instance.Get<DHTEventService>();

            DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
            TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
            DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
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