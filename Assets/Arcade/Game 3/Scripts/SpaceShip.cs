using UnityEngine;

namespace Arcade.Game_3.Scripts
{
	public class SpaceShip : MonoBehaviour
	{
		internal Rigidbody2D    rb;
		internal bool           alive;
		internal ParticleSystem explosion;
		internal LineRenderer   model;
		internal Collider2D     collider;

		void Start()
		{
			rb        = GetComponent<Rigidbody2D>();
			model     = GetComponent<LineRenderer>();
			explosion = GetComponentInChildren<ParticleSystem>();
			collider  = GetComponentInChildren<Collider2D>();
		}

		internal void Explode()
		{
			model.enabled    = false;
			collider.enabled = false;
			
			explosion.Play();
		}
	}
}
