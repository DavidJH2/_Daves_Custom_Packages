using System.Diagnostics;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using com.davidhopetech.vr.Run_Time.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif


namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class HUD : MonoBehaviour
	{
		[SerializeField] private  TMP_Dropdown        dropDown;
		[SerializeField] private  DHTXROrigin         dhtXROrigin;
		[SerializeField] internal GameObject          hudUI;
		[SerializeField] internal InputActionProperty menuButton;
		[SerializeField] internal XRRayInteractor     lefthandXRRayInteractor;

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
		}


		public void ResetXROriginPosition()
		{
			dhtXROrigin.Recenter();
		}
		
		public void VRModeSelected()
		{
			dhtXROrigin.SetVRMode(dropDown.value == 0);
		}


		public void Toggle()
		{
			_logService.Log("--------  Toggle  --------\n");
			hudUI.SetActive(!hudUI.activeSelf);
			lefthandXRRayInteractor.enabled = !hudUI.activeSelf;
		}
		
		
		private float lastMenuButtonValuel = -1f;

		void Update()
		{
			var menuButtonValue = menuButton.action.ReadValue<float>();

			if (_debugPanel) _debugPanel.SetElement(3,$"Menu Button:{menuButtonValue}","");
			
			if (menuButtonValue != lastMenuButtonValuel)
			{
				if (menuButtonValue > 0.9f)
				{
					Toggle();
					
					// StackTrace();
				}
				lastMenuButtonValuel = menuButtonValue;
			}
		}

		void StackTrace()
		{
			StackTrace   stackTrace  = new StackTrace(true); // Set 'true' to capture file information
			StackFrame[] stackFrames = stackTrace.GetFrames();

			// Look through the call stack for the first frame that has a file name
			foreach (var frame in stackFrames)
			{
				string fileName = frame.GetFileName();
				DhtDebug.Log($"File: {frame.GetFileName()}\t\tMethod: {frame.GetMethod().Name}");
			}
			

			string scriptPath = "D:\\Git Hub (D drive)\\_Daves_Custom_Packages\\Daves Custom Packages\\Assets\\com.davidhopetech.vr\\Run Time\\Scripts\\HUD.cs";
					
#if UNITY_EDITOR			
			var result = InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 83);
#endif			
		}
	}
}
