using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Extensions
{
	public static class RigidbodyExtensions
	{
		public static Vector3 GetVelocity(this Rigidbody rb)
		{
 #if UNITY_2022_1_OR_NEWER && !UNITY_2022
			return rb.linearVelocity;
#else
			return rb.velocity;
#endif
		}
		
		
		public static void SetVelocty(this Rigidbody rb, Vector3 newVelocity)
		{
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
			rb.linearVelocity = newVelocity;
#else
			rb.velocity = newVelocity;
#endif
		}
	}
}