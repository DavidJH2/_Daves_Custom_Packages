using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.run_time.scripts
{
    public class HandActuator : MonoBehaviour
    {
        public InputActionProperty pinchAnimationAction;
        public InputActionProperty gripAnimationAction;
        public Animator            handAnimator;

        void Start()
        {

        }


        void Update()
        {
            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}