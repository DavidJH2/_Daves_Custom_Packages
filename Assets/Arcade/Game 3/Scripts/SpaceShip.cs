using UnityEngine;

namespace Arcade.Game_3.Scripts
{
	public class SpaceShip : MonoBehaviour
	{
		internal Rigidbody rb;

		void Start()
		{
			rb = GetComponent<Rigidbody>();
		}
	}
}
