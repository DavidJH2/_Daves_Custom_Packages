using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction
{
	[RequireComponent(typeof(Transform))]
	public class DTHInteractable : MonoBehaviour
	{
		[SerializeField] internal float range = .08f;

		public float Dist(Vector3 pos)
		{
			var interactorPos = GetComponent<Transform>().position;
			var dis = (pos - interactorPos).magnitude;

			return dis;
		}
		
		public bool InRange(Vector3 point)
		{
			return (Dist(point)<range);
		}
	}
}
