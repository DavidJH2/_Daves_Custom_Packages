using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Extensions
{
	public static class ObjectExtentions
	{
		public static T DHTFindObjectOfType<T>(bool findObjectInactiveFlag) where T : Object
		{
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
			FindObjectsInactive findObjectInactive =
				(findObjectInactiveFlag ? FindObjectsInactive.Exclude : FindObjectsInactive.Include);
			return Object.FindFirstObjectByType<T>(findObjectInactive);
#else					
			return Object.FindObjectOfType<T>(findObjectInactiveFlag);
#endif
		}
	}
}