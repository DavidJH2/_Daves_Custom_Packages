using Codice.Client.Commands.WkTree;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.vr.Run_Time.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class HUD : MonoBehaviour
	{
		[SerializeField] private  TMP_Dropdown        dropDown;
		[SerializeField] private  DHTXROrigin         dhtXROrigin;
		[SerializeField] internal InputActionProperty menuButton;

		private DHTPlayerController  _playerController;
		private DTHLogService        _logService;
		private DebugPanel           _debugPanel;

		// Start is called before the first frame update
		void Start()
		{
			_logService        = DHTServiceLocator.Get<DTHLogService>();
			
			_debugPanel = ObjectExtentions.DHTFindObjectOfType<DebugPanel>(true);
			
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
            _playerController = FindFirstObjectByType<DHTPlayerController>();
#else
			_playerController = FindObjectOfType<DHTPlayerController>();
#endif

			if (_playerController != null)
			{
				_playerController.SetVRMode(dropDown);
			}

			// menuButton.action.performed += MenubuttonPressed;
		}


		void MenubuttonPressed(InputAction.CallbackContext context)
		{
			if (true)
			{
				
			}
		}

		public void VRModeSelected()
		{
			dhtXROrigin.SetVRMode(dropDown.value == 0);
		}

		private bool lastmenuButtonValuel = false;
		// Update is called once per frame
		void Update()
		{
			bool menuButtonValue = menuButton.action.ReadValue<bool>();
			if(_debugPanel) _debugPanel.SetElement(2,$"Menu Button Value: {menuButtonValue}", "");
			
			if (menuButtonValue != lastmenuButtonValuel)
			{
				_logService.Log($"Menu Button: {menuButtonValue}\n");
				lastmenuButtonValuel = menuButtonValue;
			}
		}
	}

}
