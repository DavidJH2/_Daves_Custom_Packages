using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.davidhopetech.core.Run_Time.Extensions
{
	public static class TransformExtensions
	{
		public static Vector2 forward2D(this Transform trans)
		{
			var up = trans.up;
			return new Vector2(up.x, up.y);
		}

		public static float Dist(this Transform trans, Transform other)
		{
			return Vector3.Distance(trans.position, other.position);
		}
	}
}