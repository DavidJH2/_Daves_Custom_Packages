using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.vr.Run_Time.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class Hud : MonoBehaviour
	{
		[SerializeField] private  TMP_Dropdown        dropDown;
		[SerializeField] private  Toggle              DebugToolsToggle;
		[SerializeField] private  DHTXROrigin         dhtXROrigin;
		[SerializeField] internal GameObject          hudUI;
		[SerializeField] internal XRRayInteractor     lefthandXRRayInteractor;
		[SerializeField] internal InputActionProperty menuButton;

		private DHTPlayerController    _playerController;
		private DHTLogService          _logService;
		private DHTDebugPanel_1_Service _dhtDebugPanel_1_Service;

		private DebugTools _debugTools;



		public void OnDebugToolsToggleValueChange(bool state)
		{
			_debugTools.Show = state;
		}
		
		
		
		void Start()
		{
			_debugTools           = ObjectExtentions.DHTFindObjectOfType<DebugTools>(true);
			DebugToolsToggle.isOn = _debugTools.Visible;
#if false
			string nullExcepton = null;
			var    a            = nullExcepton.Length;
#endif
			_logService              = DHTServiceLocator.Get<DHTLogService>();
			_dhtDebugPanel_1_Service = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
			dhtXROrigin              = ObjectExtentions.DHTFindObjectOfType<DHTXROrigin>(true);

			_playerController = ObjectExtentions.DHTFindObjectOfType<DHTPlayerController>(false);

			if (_playerController != null)
			{
				_playerController.SetVRMode(dropDown);
			}
		}
		
		public void ResetXROriginPosition()
		{
			dhtXROrigin.Recenter();
		}
		
		public void VRModeSelected()
		{
			dhtXROrigin.SetVRMode(dropDown.value == 0);
		}


		public void ToggleHUD()
		{
			_logService.Log("--------  ToggleHUD  --------\n");

			var newState = !hudUI.activeSelf; 
			
			hudUI.SetActive(newState);
			lefthandXRRayInteractor.enabled = newState;
		}
		
		
		private float lastMenuButtonValuel = -1f;

		void Update()
		{
			

			var menuButtonValue = menuButton.action.ReadValue<float>();
			if (_dhtDebugPanel_1_Service) _dhtDebugPanel_1_Service.SetElement(3,$"Menu Button:{menuButtonValue}","");
			
			if (menuButtonValue != lastMenuButtonValuel)
			{
				if (menuButtonValue > 0.9f)
				{
					ToggleHUD();
				}
				lastMenuButtonValuel = menuButtonValue;
			}
		}
	}
}
