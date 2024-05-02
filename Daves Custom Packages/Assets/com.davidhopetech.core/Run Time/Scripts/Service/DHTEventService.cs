using System.Collections.Generic;
using System.ComponentModel;
using com.davidhopetech.core.Run_Time.Extensions;
using UnityEngine;
using Component = UnityEngine.Component;

namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
    public class DHTEventService : DHTService<DHTEventService>
    {
        private static HashSet<Object> cache = new();
    
        
        public void AddNewEvent<T>() where T : Component
        {
            if (GetComponent<T>() == null)
                return;

            gameObject.AddComponent<T>();
        }
        
        public TEventType Get<TEventType>() where TEventType : MonoBehaviour
        {
            var fEvent = ObjectExtentions.DHTFindObjectOfType<TEventType>(true);
            
            if(fEvent==null)
                Debug.Log($"DHT Event '{typeof(TEventType).Name}' Not In Scene");
            
            return fEvent;
        }
    }
}
