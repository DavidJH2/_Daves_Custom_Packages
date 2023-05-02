using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{
    [Serializable]

    abstract class DHTInteractionState : MonoBehaviour
    {
        protected DHTPlayerController _controller;
        
        public abstract void UpdateState();
    }
}

