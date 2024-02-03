using UnityEngine;

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
}
