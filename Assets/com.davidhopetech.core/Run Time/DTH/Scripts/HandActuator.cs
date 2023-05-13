using System;
using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.run_time.scripts
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
            EventService     = DHTServiceLocator.DhtEventService;
            
            DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
            TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
            DebugValue1Event = EventService.dhtUpdateDebugValue1Event;

            DebugValue1Event.Invoke("This Works");
        }

        void Start()
        {

        }


        void Update()
        {
            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);
            handAnimator.SetFloat("Grip", gripValue);

            DebugValue1Event.Invoke($"Trigger: {triggerValue}\nGrip: {gripValue}");
            if (triggerValue > 0.2f)
            {
                
            }
        }
    }
}