using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DHTBootstrapper : MonoBehaviour
{
	public static readonly   int    ServicesSceneBuildIndex = 0;

	[SerializeField] private string firstSceneName;

	async void Start()
	{
		await LoadAndSetActiveScene();
	}


	async Task LoadAndSetActiveScene()
	{
		var servicesSceneName = SceneManager.GetSceneByBuildIndex(ServicesSceneBuildIndex).name;
		if (gameObject.scene.name != servicesSceneName)
		{
			throw new Exception($"Class: {this.name} should ONLY be in {SceneManager.GetSceneByBuildIndex( DHTBootstrapper.ServicesSceneBuildIndex).name}");
		}

		var activeScene = SceneManager.GetActiveScene();

		if (activeScene.name == servicesSceneName)
		{
			// Load first scene and make active
			var asyncLoad = SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
		 
			while (!asyncLoad.isDone)
			{
				await Task.Yield();
			}
			
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstSceneName));
		}
		else
		{
			/*
			// Activate Bootstrapper
			var lastActiveScene     = activeScene;
			var lastActiveSceneName = lastActiveScene.name;
			
			Scene loadedScene = SceneManager.GetSceneByName(ServicesSceneName);
			SceneManager.SetActiveScene(loadedScene);
				
			// Unload and reload scene to activate it after the Bootstrapper is loaded
			SceneManager.UnloadSceneAsync(lastActiveScene);

			var asyncLoad = SceneManager.LoadSceneAsync(lastActiveSceneName, LoadSceneMode.Additive);
			while (!asyncLoad.isDone)
			{
				await Task.Yield();
			}
			
			var scene = SceneManager.GetSceneByName(lastActiveSceneName);
			SceneManager.SetActiveScene(scene);
			*/
		}
	}
}
