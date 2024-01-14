using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Misc
{
    public class Singleton : MonoBehaviour 
    {
        private static Singleton _instance;

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Singleton instance not found in the scene!");
                    // Debug.LogError("Singleton instance not found in the scene!");
                }
                
                return _instance;
            }
        }    
    
        private void Awake() 
        { 
            if (_instance != null && _instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                _instance = this; 
            } 
        }}
}