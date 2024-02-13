using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class DHTUserPresence : MonoBehaviour
	{
		[SerializeField] private GameObject vrCamGO;
		[SerializeField] private GameObject pancakeCamGO;


		public  UnityEvent<GameObject> CameraChange = new();
		private GameObject             _currentCameraGO;

		public GameObject CurrentCameraGO
		{
			get { return _currentCameraGO; }
		}


		private void Start()
		{
			InitializeServices();	
			InitCams();
			GetCurrentCamera();
		}

		void InitCams()
		{
#if PLATFORM_ANDROID && !UNITY_EDITOR 
			ChangeCamera(vrCamGO);

			/*
			 vrCamGO.SetActive(true);
			pancakeCamGO.SetActive(false);
			*/
#else
			ChangeCamera(pancakeCamGO);
			
			/*
			vrCamGO.SetActive(false);
			pancakeCamGO.SetActive(true);
			*/
#endif
		}


		private void GetCurrentCamera()
		{
			if (vrCamGO.activeSelf)
			{
				_currentCameraGO = vrCamGO;
			}
			else if(pancakeCamGO.activeSelf)
			{
				_currentCameraGO = pancakeCamGO;
			}
		}

		private void InitializeServices()
		{
			var service = DHTServiceLocator.Get<DHTHMDService>();
			if (service)
			{
				service.UserPresenceEvent.AddListener(OnUserPresence);
				service.HMDFirstMountEvent.AddListener(OnHMDFirstMount);
			}
			
		}

		void OnHMDFirstMount()
		{
		}

		public void OnUserPresence(bool hmdMounted)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			if (hmdMounted)
			{
				Debug.Log("User Presence");
				ChangeCamera(vrCamGO);
			}
			else
			{
				Debug.Log("No User Presence");
				
				ChangeCamera(pancakeCamGO);
			}
#else
#if PLATFORM_ANDROID
		vrCamGO.SetActive(true);
		pancakeCamGO.SetActive(false);
#endif
#endif
		}

		private void ChangeCamera(GameObject newCameraGo)
		{
			if(_currentCameraGO) _currentCameraGO.SetActive(false);
			newCameraGo.SetActive(true);
			_currentCameraGO = newCameraGo;

			Camera   camera      = newCameraGo.GetComponent<Camera>();
			Canvas[] _canvasList = ObjectExtentions.DHTFindObjectsByType<Canvas>(true);
			foreach (var canvas in _canvasList)
			{
				canvas.worldCamera = camera;
			}

			TrackedDevicePhysicsRaycaster[] _trackedDevicePhysicsRaycasterList = ObjectExtentions.DHTFindObjectsByType<TrackedDevicePhysicsRaycaster>(true);
			foreach (var obj in _trackedDevicePhysicsRaycasterList)
			{
				obj.SetEventCamera(camera);
			}

			
			CameraChange.Invoke(newCameraGo);
		}
	}
}