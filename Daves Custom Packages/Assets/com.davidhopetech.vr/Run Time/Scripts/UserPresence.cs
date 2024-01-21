using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class UserPresence : MonoBehaviour
    {
        [SerializeField] private GameObject vrCam;
        [SerializeField] private GameObject cam;


        public UnityEvent<GameObject> CameraChange = new UnityEvent<GameObject>();
        
        private void Start()
        {
            var service = DHTServiceLocator.Get<DHTHMDService>();
            if(service) service.UserPresence.AddListener(OnUserPresence);
        }

        public void OnUserPresence(bool hmdMounted)
        {
#if UNITY_STANDALONE_WIN
            if (hmdMounted)
            {
                Debug.Log("User Presence");
                vrCam.SetActive(true);
                cam.SetActive(false);
                
                CameraChange.Invoke(vrCam);
            }
            else
            {
                Debug.Log("No User Presence");
                vrCam.SetActive(false);
                cam.SetActive(true);
                
                CameraChange.Invoke(cam);
            }
#endif
#if PLATFORM_ANDROID
        vrCam.SetActive(true);
        cam.SetActive(false);
#endif
        }
    }
}
