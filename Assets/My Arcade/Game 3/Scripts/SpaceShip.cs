using UnityEngine;

namespace Arcade.Game_3.Scripts
{
	public class SpaceShip : MonoBehaviour
	{
		[SerializeField] private GameObject thrustImage;

		internal                 bool           _alive;
		internal                 Rigidbody2D    rb;
		internal                 ParticleSystem explosion;
		[SerializeField] private LineRenderer   model;
		internal                 Collider2D     collider_;

		void Awake()
		{
			rb        = GetComponent<Rigidbody2D>();
			//model     = GetComponentInChildren<LineRenderer>();
			explosion = GetComponentInChildren<ParticleSystem>();
			collider_  = GetComponentInChildren<Collider2D>();
		}


		internal bool alive
		{
			get
			{
				return _alive;
			}

			set
			{
				_alive           = value;
				model.enabled    = value;
				collider_.enabled = value;
				thrustImage.SetActive(value);
			}
		}

		internal void Explode()
		{
			alive            = false;
			explosion.Play();
		}
	}
}
