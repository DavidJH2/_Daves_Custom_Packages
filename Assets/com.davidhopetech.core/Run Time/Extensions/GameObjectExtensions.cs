using UnityEngine;

public static class TransformExtensions
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


public static class Vector3Extentions
{
	public static void Clamp(this Vector3 vec, float min, float max)
	{
		var mag = Mathf.Clamp(vec.magnitude, max, max);

		vec = vec.normalized * mag;
	}
}