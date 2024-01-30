using System;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DHTServicesLoader : MonoBehaviour
{
#if UNITY_EDITOR
	ServicesSceneSettings servicesSceneSettings
	{
		get
		{
			ServicesSceneSettings[] _settingsResources = Resources.LoadAll<ServicesSceneSettings>("");
			if (_settingsResources.Length > 1) throw new Exception($"There should only be one 'Boot Strapper Scene Settings' Resource");

			return _settingsResources[0];
		}
	}
	
	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		Debug.Log(DTH.DecoratedMethodeInfo(gameObject));
	}

	private void Start()
	{
		Debug.Log(DTH.DecoratedMethodeInfo(gameObject));
	}

	private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
	{
		Debug.Log($"{DTH.DecoratedMethodeInfo(gameObject)}                Scene Loaded: {loadedScene.name}      <--------------------");
		if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
		{
			Debug.Log("------  All Scenes Loaded  ------");
			TryLoadServicesScene();
		}
	}

	void TryLoadServicesScene()
	{
		var servicesSceneName = servicesSceneSettings.ServicesSceneName;
		var servicesSceneBuildIndex  = GetServicesSceneBuildIndex(servicesSceneName);
		var bootstrapper = GetBootStrapper();

		var editorServicesScene = SceneManager.GetSceneByBuildIndex(servicesSceneBuildIndex);

		if(!editorServicesScene.IsValid())
		{
			SceneManager.LoadSceneAsync(servicesSceneBuildIndex, LoadSceneMode.Additive);
		}
	}

	int GetServicesSceneBuildIndex(string sceneName)
	{
		var editorBuildScenes = EditorBuildSettings.scenes;
		var buildIndex        = -1;
		for (int i = 0; i < editorBuildScenes.Length; i++)
		{
			var scene = editorBuildScenes[i];
			if (scene.path.Contains(sceneName))
			{
				Debug.Log($"Services Scene found at path: {scene.path}");
				if (buildIndex != -1)
				{
					throw new Exception($"Should only be one scene called {sceneName}");
				}

				buildIndex = i;
			}
		}

		return buildIndex;
	}

	DHTBootstrapper GetBootStrapper()
	{
		var bootstrappers = ObjectExtentions.DHTFindObjectsByType<DHTBootstrapper>();

		if (bootstrappers.Length == 0) return null;
		if (bootstrappers.Length > 1) Debug.Log("------  Too Many Bootstrappers  ------");

		var bootstrapperScene = bootstrappers[0].gameObject.scene;

		return bootstrappers[0];
	}
#endif
}
