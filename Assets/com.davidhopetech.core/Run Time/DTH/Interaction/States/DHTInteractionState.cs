using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{

    [Serializable]
    abstract class DHTInteractionState : MonoBehaviour
    {
        internal float GripThreshold = .1f;
    
        protected DHTEventContainer   EventContainer ;
        protected DHTPlayerController Controller;
        

        internal void Awake()
        {
            EventContainer = FindObjectOfType<DHTEventContainer>().GetComponent<DHTEventContainer>();
            Controller     = GetComponent<DHTPlayerController>();
        }

        public abstract void UpdateState();
    }
}

