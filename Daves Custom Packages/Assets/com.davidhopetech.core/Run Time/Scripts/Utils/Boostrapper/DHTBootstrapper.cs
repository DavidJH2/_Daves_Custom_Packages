using System;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DHTBootstrapper : MonoBehaviour
{
	[SerializeField] private string firstSceneName;

	void Start()
	{
		var serviceLoader                    = ObjectExtentions.DHTFindObjectOfType<DHTServicesLoader>();
		var serviceLoaderInBootstrapperScene = (serviceLoader.gameObject.scene == gameObject.scene);
		if (serviceLoaderInBootstrapperScene)
			throw new Exception(
				$"{typeof(DHTServicesLoader).Name} should not be in the same scene as {typeof(DHTBootstrapper).Name}");

		var firstScene           = SceneManager.GetSceneByName(firstSceneName);
		var firstSceneIsNotValid = !firstScene.IsValid();

		if (firstSceneIsNotValid) throw new Exception($"Fist scene not be found");

		var bootstrappers = ObjectExtentions.DHTFindObjectsByType<DHTBootstrapper>();
		if (bootstrappers.Length > 1) Debug.Log("------  Too Many Bootstrappers  ------");
		var bootstrapper = bootstrappers[0];

		if (SceneManager.GetActiveScene() == bootstrapper.gameObject.scene)
		{
			var firstSceneNotLoaded = !firstScene.isLoaded;
			if (firstSceneNotLoaded)
			{
				SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
			}
		}
	}
}
