using UnityEngine;


namespace com.davidhopetech.core.Run_Time.Extensions
{
	public static class ArticulateBodyExtensions
	{
		public static Vector3 GetVelocity(this ArticulationBody ab)
		{
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
			return ab.linearVelocity;
#else
			return ab.velocity;
#endif
		}
		
		
		public static void SetVelocty(this ArticulationBody ab, Vector3 newVelocity)
		{
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
			ab.linearVelocity = newVelocity;
#else
			ab.velocity = newVelocity;
#endif
		}
	}
}