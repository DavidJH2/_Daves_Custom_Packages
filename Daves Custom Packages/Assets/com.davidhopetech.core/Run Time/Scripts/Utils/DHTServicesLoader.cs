using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DHTServicesLoader : MonoBehaviour
{
	void Awake()
	{
		if (gameObject.scene.name == DHTBootstrapper.ServicesSceneName)
		{
			throw new Exception($"Class: {this.name} should not be in {DHTBootstrapper.ServicesSceneName}");
		}

		if (SceneManager.GetActiveScene().name == gameObject.scene.name)
		{
			SceneManager.LoadSceneAsync(DHTBootstrapper.ServicesSceneName, LoadSceneMode.Additive);
		}
	}
}
