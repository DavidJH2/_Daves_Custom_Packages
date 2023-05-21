using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Extensions
{
	public static class Vector3Extentions
	{
		public static void Clamp(this Vector3 vec, float min, float max)
		{
			var mag = Mathf.Clamp(vec.magnitude, max, max);

			vec = vec.normalized * mag;
		}
	}
}