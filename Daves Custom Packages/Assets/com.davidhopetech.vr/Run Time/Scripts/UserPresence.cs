using System;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class UserPresence : MonoBehaviour
	{
		[SerializeField] private GameObject vrCamGO;
		[SerializeField] private GameObject pancakeCamGO;


		public  UnityEvent<GameObject> CameraChange = new();
		private Canvas[]               _canvasList;
		private GameObject             currentCameraGO;
		
		
		private void Start()
		{
			_canvasList = ObjectExtentions.DHTFindObjectsByType<Canvas>();
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
				currentCameraGO = vrCamGO;
			}
			else if(pancakeCamGO.activeSelf)
			{
				currentCameraGO = pancakeCamGO;
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
			if(currentCameraGO) currentCameraGO.SetActive(false);
			newCameraGo.SetActive(true);
			currentCameraGO = newCameraGo;
			
			foreach (var canvas in _canvasList)
			{
				canvas.worldCamera = newCameraGo.GetComponent<Camera>();
			}
			CameraChange.Invoke(pancakeCamGO);
		}
	}
}
