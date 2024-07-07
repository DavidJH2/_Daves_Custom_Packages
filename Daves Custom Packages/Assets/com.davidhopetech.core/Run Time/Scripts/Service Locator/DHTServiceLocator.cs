

using System;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
	public class DHTServiceLocator : Singleton<DHTServiceLocator>
	{
		private static Dictionary<Type, MonoBehaviour> cachedServices = new();


		public static TServiceType Get<TServiceType>(bool supressWarnings = false) where TServiceType : MonoBehaviour
		{
			cachedServices.TryGetValue(typeof(TServiceType), out var service);

			if (service)
			{
				// DHTDebug.Log("------  Found Cached DHTService  ------");
				return (TServiceType)service;
			}

			// DHTDebug.Log("------  Locating DHTService  ------");
			var services = ObjectExtentions.DHTFindObjectsByType<TServiceType>(true);

			if (services.Length == 0)
			{
				if (!supressWarnings)
				{
					var typeName = typeof(TServiceType).Name;
					Debug.Log($"DHT DHTService '{typeName}' Not In Scene");
				}
				return null; //
			}

			if (!supressWarnings && services.Length > 1) Debug.Log($"There should only be one DHT DHTService '{typeof(TServiceType).Name}' In Scene");

			service                              = services[0];
			cachedServices[typeof(TServiceType)] = service;
			return (TServiceType)service;
		}


		public static bool IsServiceCached<TServiceType>(bool supressWarnings = false) where TServiceType : DHTService<TServiceType>
		{
			cachedServices.TryGetValue(typeof(TServiceType), out var service);

			return service is not null;
		}
	}
}

