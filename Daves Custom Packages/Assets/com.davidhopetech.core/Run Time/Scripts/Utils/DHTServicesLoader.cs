using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
