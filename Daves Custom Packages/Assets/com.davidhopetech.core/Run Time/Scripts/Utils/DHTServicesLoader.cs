using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DHTServicesLoader : MonoBehaviour
{
	void Awake()
	{
		if (gameObject.scene.buildIndex == DHTBootstrapper.ServicesSceneBuildIndex) {
			var ServicesScene     = SceneManager.GetSceneByBuildIndex(DHTBootstrapper.ServicesSceneBuildIndex);
			var ServicesSceneName = ServicesScene.name;
			throw new Exception($"Class: {this.name} should not be in {ServicesSceneName}");
		}

		if (SceneManager.GetActiveScene().name == gameObject.scene.name)
		{
			SceneManager.LoadSceneAsync(DHTBootstrapper.ServicesSceneBuildIndex, LoadSceneMode.Additive);
		}
	}
}
