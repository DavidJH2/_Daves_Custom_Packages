using System;
using System.Reflection;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DHTBootstrapper : MonoBehaviour
{
	[SerializeField] private string firstSceneName;

	private void Awake()
	{
		Debug.Log(DTH.DecoratedMethodeInfo(gameObject));
		SceneManager.sceneLoaded += OnSceneLoaded;
		ErrorChecking();
	}

	void Start()
	{
		Debug.Log(DTH.DecoratedMethodeInfo(gameObject));
	}

	private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
	{
		Debug.Log($"{DTH.DecoratedMethodeInfo(gameObject)}                Scene Loaded: {loadedScene.name}      <--------------------");
		if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
		{
			Debug.Log("------  All Scenes Loaded  ------");
			TryLoadFirstScene();
		}

		var bootstrapperScene = gameObject.scene;
	}


	void ErrorChecking()
	{
		if(ObjectExtentions.DHTFindObjectsByType<DHTBootstrapper>().Length>1)
			Debug.Log($" ----------------------------------------------------  There should only be one {nameof(DHTBootstrapper)}  ----------------------------------------------------");
		if (gameObject.scene.buildIndex != 0)
			Debug.Log($"---------------------------------------------------- Scene {gameObject.scene} should be Scene 0 in Build Settings  ----------------------------------------------------");
	
		var serviceLoaders = ObjectExtentions.DHTFindObjectsByType<DHTServicesLoader>();
		foreach (var serviceLoader in serviceLoaders)
		{
			var serviceLoaderInBootstrapperScene = (serviceLoader.gameObject.scene == gameObject.scene);
			if (serviceLoaderInBootstrapperScene) Debug.Log($"----------------------------------------------------  {nameof(DHTServicesLoader)} should not be in the same scene as {nameof(DHTBootstrapper)}" + 
			                                                "----------------------------------------------------");
		}
	}


	private void TryLoadFirstScene()
	{
		Debug.Log(DTH.DecoratedMethodeInfo(gameObject));

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

