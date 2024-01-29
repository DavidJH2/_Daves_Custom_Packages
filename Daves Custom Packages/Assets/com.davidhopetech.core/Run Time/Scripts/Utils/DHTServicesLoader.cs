using System;
using Codice.CM.SEIDInfo;
using com.davidhopetech.core.Run_Time.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DHTServicesLoader : MonoBehaviour
{
	void Start()
	{
		BootstrapperSceneSettings[] _settingsResources = Resources.LoadAll<BootstrapperSceneSettings>("");
		if (_settingsResources.Length > 1)
			throw new Exception($"There should only be one 'Boot Strapper Scene Settings' Resource");

		var _settings = _settingsResources[0];
		//var bootstrapperBuildIndex = SceneManager.getsc
		var servicesScene     = this.gameObject.scene;
		var servicesSceneName = servicesScene.name;
		
		var bootstrappers      = ObjectExtentions.DHTFindObjectsByType<DHTBootstrapper>();
		if (bootstrappers.Length > 0)
		{
			if (bootstrappers.Length > 1) Debug.Log("------  Too Many Bootstrappers  ------");

			var bootstrapperScene = bootstrappers[0].gameObject.scene;
			if (bootstrapperScene.name != servicesSceneName)
				throw new Exception($"{typeof(DHTBootstrapper).Name} should only be in Services Scene");

			if (gameObject.scene == bootstrapperScene)
				throw new Exception($"{typeof(DHTServicesLoader).Name} should not be in the same scene as {typeof(DHTBootstrapper).Name}");
			
			gameObject.scene.
			
			if(!bootstrapperScene.isLoaded)
			{
				SceneManager.LoadSceneAsync(bootstrapperScene.buildIndex, LoadSceneMode.Additive);
			}
		}
	}
}
