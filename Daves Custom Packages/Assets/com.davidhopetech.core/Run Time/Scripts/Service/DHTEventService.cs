using System.Collections.Generic;
using System.ComponentModel;
using com.davidhopetech.core.Run_Time.Extensions;
using UnityEngine;
using Component = UnityEngine.Component;

namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
    public class DHTEventService : MonoBehaviour
    {
        private static HashSet<Object> cache = new();
        
        // public         DHTUpdateDebugValue1Event   dhtUpdateDebugValue1Event   = new DHTUpdateDebugValue1Event();
        // public         DHTUpdateDebugTeleportEvent dhtUpdateDebugTeleportEvent = new DHTUpdateDebugTeleportEvent();
        // public DHTUpdateDebugMiscEvent     dhtUpdateDebugMiscEvent     = new DHTUpdateDebugMiscEvent();
    
        
        public void AddNewEvent<T>() where T : Component
        {
            if (GetComponent<T>() == null)
                return;

            gameObject.AddComponent<T>();
        }
        
        public TEventType Get<TEventType>() where TEventType : Object
        {
            var Event = ObjectExtentions.DHTFindObjectOfType<TEventType>(true);
            
            if(Event==null)
                Debug.Log($"DHT Event '{typeof(TEventType).Name}' Not In Scene");
            
            return ObjectExtentions.DHTFindObjectOfType<TEventType>(true);
        }
    }
}
