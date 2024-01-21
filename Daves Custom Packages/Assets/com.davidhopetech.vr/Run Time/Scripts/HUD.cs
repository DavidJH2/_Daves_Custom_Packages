using com.davidhopetech.vr.Run_Time.Scripts.Interaction;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropDown;
        [SerializeField] private DHTXROrigin  dhtXROrigin;
    
        private DHTPlayerController _playerController;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
            _playerController = FindFirstObjectByType<DHTPlayerController>();
#else					
            _playerController = FindObjectOfType<DHTPlayerController>();
#endif

            if (_playerController != null)
            {
                _playerController.SetVRMode(dropDown);
            }
        }

        public void VRModeSelected()
        {
            dhtXROrigin.SetVRMode(dropDown.value==0);
        }
        // Update is called once per frame
        void Update()
        {
        }
    }
}
