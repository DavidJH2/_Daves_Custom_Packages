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
}
