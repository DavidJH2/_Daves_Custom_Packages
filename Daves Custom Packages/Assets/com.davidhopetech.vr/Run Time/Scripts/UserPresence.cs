using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class UserPresence : MonoBehaviour
    {
        [SerializeField] private GameObject vrCam;
        [SerializeField] private GameObject cam;
    
        public void OnUserPresence(bool hmdMounted)
        {
#if UNITY_STANDALONE_WIN
            if (hmdMounted)
            {
                Debug.Log("User Presence");
                vrCam.SetActive(true);
                cam.SetActive(false);
            }
            else
            {
                Debug.Log("No User Presence");
                vrCam.SetActive(false);
                cam.SetActive(true);
            }
#endif
#if PLATFORM_ANDROID
        vrCam.SetActive(true);
        cam.SetActive(false);
#endif
        }
    }
}
