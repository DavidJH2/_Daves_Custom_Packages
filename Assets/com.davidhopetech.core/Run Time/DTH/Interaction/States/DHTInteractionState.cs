using System;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{

    [Serializable]
    abstract class DHTInteractionState : MonoBehaviour
    {
        internal float GripThreshold = .1f;
    
        protected DHTEventService     EventService ;
        protected DHTPlayerController Controller;
        

        internal void Awake()
        {
            EventService = DHTServiceLocator.DhtEventService;
            Controller     = GetComponent<DHTPlayerController>();
        }

        public abstract void UpdateState();
    }
}

