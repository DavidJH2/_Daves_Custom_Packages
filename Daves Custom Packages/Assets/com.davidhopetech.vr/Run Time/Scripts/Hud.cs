using System;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.vr.Run_Time.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class Hud : MonoBehaviour
	{
		public UnityEvent<bool> HudVisableEvent;
        
		[SerializeField] private  TMP_Dropdown        dropDown;
		[SerializeField] private  Toggle              debugToolsToggle;
		[SerializeField] private  DHTXROrigin         dhtXROrigin;
		[SerializeField] public   GameObject          hudUI;
		[SerializeField] internal XRRayInteractor     lefthandXRRayInteractor;
		[SerializeField] internal InputActionProperty menuButton;

		private DHTPlayerController     _playerController;
		private DHTLogService           _logService;
		private DHTDebugPanel_1_Service _dhtDebugPanel_1_Service;
		private DebugTools              _debugTools;
		public  UnityEvent<Single>      VoulumeChangeEvent;

		public void OnDebugToolsToggleValueChange(bool state)
		{
			_debugTools.Visible = state;
		}
		
		
		
		void Start()
		{
			_debugTools           = ObjectExtentions.DHTFindObjectOfType<DebugTools>(true);
			debugToolsToggle.isOn = _debugTools.Visible;
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


		public void OnVolumeChange(Single newVolume)
		{
			VoulumeChangeEvent.Invoke(newVolume);
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
			Visible = newState;
		}


		public bool Visible
		{
			get => hudUI.activeSelf;
			set
			{
				hudUI.SetActive(value);
				HudVisableEvent.Invoke(value);
			}
		}
	}
}
