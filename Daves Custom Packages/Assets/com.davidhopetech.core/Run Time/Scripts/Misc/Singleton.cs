using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Misc
{
    public class Singleton : MonoBehaviour 
    {
        public static Singleton Instance { get; private set; }
    
    
        private void Awake() 
        { 
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }}
}