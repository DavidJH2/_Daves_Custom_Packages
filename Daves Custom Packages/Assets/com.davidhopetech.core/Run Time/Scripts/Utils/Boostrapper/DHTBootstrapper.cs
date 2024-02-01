using System;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DHTBootstrapper : MonoBehaviour
{
	[SerializeField] private string firstSceneName;

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		ErrorChecking();
	}

	void Start()
	{
	}

	private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
	{
		DhtDebug.LogTag($"  ----------->  Scene Loaded:",this);
		if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
		{
			DhtDebug.LogTag("------  All Scenes Loaded  ------",this);
			OnAllScenesLoaded();
		}

		var bootstrapperScene = gameObject.scene;
	}


	void ErrorChecking()
	{
		if(ObjectExtentions.DHTFindObjectsByType<DHTBootstrapper>().Length>1)
			Debug.Log($" ----------------------------------------------------  There should only be one {nameof(DHTBootstrapper)}  ----------------------------------------------------");
		if (gameObject.scene.buildIndex != 0)
			throw new Exception($"---------------------------------------------------- Scene {gameObject.scene} Must be Scene 0 in Build Settings  ----------------------------------------------------");
	
		var serviceLoaders = ObjectExtentions.DHTFindObjectsByType<DHTServicesLoader>();
		foreach (var serviceLoader in serviceLoaders)
		{
			var serviceLoaderInBootstrapperScene = (serviceLoader.gameObject.scene == gameObject.scene);
			if (serviceLoaderInBootstrapperScene) Debug.Log($"----------------------------------------------------  {nameof(DHTServicesLoader)} should not be in the same scene as {nameof(DHTBootstrapper)}" + 
			                                                "----------------------------------------------------");
		}
	}

	void OnAllScenesLoaded()
	{
		TryLoadFirstScene();
	}

	private void TryLoadFirstScene()
	{
		var firstScene = SceneManager.GetSceneByName(firstSceneName);
		{
			var firstSceneNotLoaded = (firstScene == null || !firstScene.isLoaded);
			if (firstSceneNotLoaded)
			{
				SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
			}
		}
	}
}

