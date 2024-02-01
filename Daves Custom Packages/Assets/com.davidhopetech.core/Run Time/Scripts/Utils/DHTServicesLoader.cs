using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DHTServicesLoader : MonoBehaviour
{
	private Object dummy = new();
	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start()
	{
	}

	private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
	{
		DhtDebug.LogTag($"  ----------->  Scene Loaded:",this);
		if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
		{
			DhtDebug.LogTag("------  All Scenes Loaded  ------", this);
			OnAllScenesLoaded();
		}
	}

	void OnAllScenesLoaded()
	{
		TryLoadServicesScene();
	}


	void TryLoadServicesScene()
	{
		var servicesSceneBuildIndex = 0;
		var editorServicesScene     = SceneManager.GetSceneByBuildIndex(servicesSceneBuildIndex);

		if (!editorServicesScene.IsValid())
		{
			SceneManager.LoadSceneAsync(servicesSceneBuildIndex, LoadSceneMode.Additive);
		}
	}
}
