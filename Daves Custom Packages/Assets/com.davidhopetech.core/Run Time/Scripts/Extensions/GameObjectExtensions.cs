using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameObjectExtensions
{
	public static void DisableAllColliders(this GameObject go)
	{
		var colliders = go.GetComponentsInChildren<Collider>();
		
		foreach (var collider in colliders)
		{
			collider.enabled = false;
		}
	}
	
	public static T[] FindObjectsOfTypeWithInterface<T>() where T : class
	{
		List<T>         objectsWithInterface = new List<T>();
		MonoBehaviour[] allObjects           = GameObject.FindObjectsOfType<MonoBehaviour>();

		foreach (MonoBehaviour obj in allObjects)
		{
			T component = obj as T;
			if (component != null)
			{
				objectsWithInterface.Add(component);
			}
		}

		return objectsWithInterface.ToArray();
	}
	
	public static void EnableAllColliders(this GameObject go)
	{
		var colliders = go.GetComponentsInChildren<Collider>();
		
		foreach (var collider in colliders)
		{
			collider.enabled = true;
		}
	}
	
	
	public static string GetPath(this GameObject gameObject)
	{
		if (gameObject == null) return "";
		// Initialize the path with the name of the input GameObject
		string path = gameObject.name;
		// Start with the current transform
		Transform current = gameObject.transform;

		// Loop through the parent objects and prepend their names to the path
		while (current.parent != null)
		{
			// Move to the parent transform
			current = current.parent;
			// Prepend the parent name to the path
			path = current.name + "/" + path;
		}

		return path;
	}
	
	
	// Generic method to find all GameObjects of a type, including inactive ones
	public static List<T> FindAllComponentsOfType<T>() where T : Component
	{
		List<T> componentsList = new List<T>();

		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (!scene.isLoaded)
				continue;

			// Get all root GameObjects in the scene
			GameObject[] rootObjects = scene.GetRootGameObjects();

			foreach (GameObject rootObj in rootObjects)
			{
				// Get components of type T in root object and its children, including inactive ones
				T[] components = rootObj.GetComponentsInChildren<T>(true);
				if (components.Length > 0)
				{
					componentsList.AddRange(components);
				}
			}
		}

		return componentsList;
	}
}
