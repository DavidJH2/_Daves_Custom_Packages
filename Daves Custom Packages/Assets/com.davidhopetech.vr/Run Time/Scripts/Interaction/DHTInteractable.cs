using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction
{
	[RequireComponent(typeof(Transform))]
	public class DHTInteractable : MonoBehaviour
	{
		[SerializeField] internal bool  active = true;
		[SerializeField] internal float range = .08f;

		public float Dist(Vector3 pos)
		{
			var interactorPos = GetComponent<Transform>().position;
			var dis = (pos - interactorPos).magnitude;

			return dis;
		}
		
		public bool InRange(Vector3 point)
		{
			var dist = Dist(point);
			//Debug.Log($"Distance: {dist}\t\tRange: {range} ");
			//if(dist<range)
				//Debug.Log("In Range");
			return (dist<range);
		}

		public virtual void Activate()
		{
			
		}
	}
}
