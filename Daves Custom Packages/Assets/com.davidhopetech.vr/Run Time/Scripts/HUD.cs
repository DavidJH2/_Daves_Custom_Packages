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
		[SerializeField] internal GameObject          hudUI;
		[SerializeField] internal InputActionProperty menuButton;

		private DHTPlayerController _playerController;
		private DHTLogService       _logService;
		private DebugPanel          _debugPanel;

		// Start is called before the first frame update
		void Start()
		{
			_logService = DHTServiceLocator.Get<DHTLogService>();
			_debugPanel = ObjectExtentions.DHTFindObjectOfType<DebugPanel>(true);
			dhtXROrigin = ObjectExtentions.DHTFindObjectOfType<DHTXROrigin>(true);

			_playerController = ObjectExtentions.DHTFindObjectOfType<DHTPlayerController>(false);

			if (_playerController != null)
			{
				_playerController.SetVRMode(dropDown);
			}

			// menuButton.action.performed += MenubuttonPressed;
		}


		public void ResetXROriginPosition()
		{
			dhtXROrigin.ResetPosition();
		}
		
		public void VRModeSelected()
		{
			dhtXROrigin.SetVRMode(dropDown.value == 0);
		}


		public void Toggle(InputAction.CallbackContext context)
		{
			_logService.Log("--------  Toggle  --------\n");
			if (context.performed)
			{
				hudUI.SetActive(!hudUI.activeSelf);
			}
		}
		
		
		private float lastmenuButtonValuel = -1f;

		void Update()
		{
			var menuButtonValue = menuButton.action.ReadValue<float>();

			if (_debugPanel) _debugPanel.SetElement(3,$"Menu Button:{menuButtonValue}","");
			
			if (menuButtonValue != lastmenuButtonValuel)
			{
				if(menuButtonValue > 0.9f) hudUI.SetActive(!hudUI.activeSelf);
				lastmenuButtonValuel = menuButtonValue;
			}
		}
	}
}
