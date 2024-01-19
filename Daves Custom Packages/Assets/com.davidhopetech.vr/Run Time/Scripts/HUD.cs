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
            _playerController = FindObjectOfType<DHTPlayerController>();

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
