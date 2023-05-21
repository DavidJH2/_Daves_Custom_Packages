using UnityEngine;

namespace Arcade.Game_3.Scripts
{
	public class SpaceShip : MonoBehaviour
	{
		internal Rigidbody2D rb;

		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}
	}
}
