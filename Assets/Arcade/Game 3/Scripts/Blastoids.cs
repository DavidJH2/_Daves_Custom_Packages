using System;
using Arcade.Game_3.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class Blastoids : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private GameObject rockPrefab;
	[SerializeField] private Transform  bulletStartPos;
	[SerializeField] private float      bulletSpeed    = 5;
	[SerializeField] private float      bulletLifeTime = 1;
	
	[SerializeField] private GameObject thrustImage;
	[SerializeField] private float      turnThreshold;
	[SerializeField] private float      _turnRate;
	[SerializeField] private float      _thrust;
	[SerializeField] private float      maxVelocity;

	[SerializeField] private Transform screenTopRight;
	[SerializeField] private Transform screenBottomLeft;
	
	[SerializeField] private DTHJoystick dthJoystick;
	[SerializeField] private DTHButton   thrustButton;
	[SerializeField] private DTHButton   fireButton;

	internal SpaceShip SpaceShip;
	private  float     turnRate;
	private  bool      lastFireButtonIsPressed;

	void Start()
	{
		dthJoystick.JoyStickEvent += OnJoystick;

		if (!SpaceShip)
		{
			SpaceShip = transform.parent.GetComponentInChildren<SpaceShip>();
		}

		InitalizeRocks();
	}

	private void OnJoystick(float arg1, float arg2)
	{
		turnRate = 0.0f;
		var coeef = 4; 

		if (arg1 > turnThreshold)
		{
			// turnRate = -_turnRate;
			turnRate = -arg1 * coeef;
		}

		if (arg1 < -turnThreshold)
		{
			// turnRate = _turnRate;
			turnRate = -arg1 * coeef;
		}
	}


	internal void InitalizeRocks()
	{
		for (var i = 0; i < 1; i++)
		{
			var rockGO  = Instantiate(rockPrefab);
			var rock    = rockGO.GetComponent<DTHLineRenderer>();
			var points  = rock.points;
			var count   = rock.points.Length;
			var radStep = 360 / count * Mathf.Deg2Rad;

			var rads = 0.0f;
			for (var j = 0; j < count; j++)
			{
				var x = Mathf.Sin(rads);
				var y = Mathf.Cos(rads);
				points[j] =  new Vector3(x, y, 0);
				rads      += radStep;
			}

			rock.points = points;
		}
	}

	private void FixedUpdate()
	{
		UpdateShip();
	}

	private void UpdateShip()
	{
		// Fire
		var fireButtonIsPressed = fireButton.isPressed;
		if (fireButtonIsPressed && !lastFireButtonIsPressed)
		{
			var newBulletGO = Instantiate(bulletPrefab, bulletStartPos.position, quaternion.identity);
			var newBullet   = newBulletGO.GetComponent<Bullet>();
			newBullet.time       = bulletLifeTime;
			newBullet.gameEngine = this;
			
			var rb          = newBulletGO.GetComponent<Rigidbody>();
			rb.velocity = SpaceShip.rb.velocity + bulletSpeed * SpaceShip.transform.up;
		}

		lastFireButtonIsPressed = fireButtonIsPressed;
		
		// Thrust
		var thrusting   = thrustButton.isPressed;
		var thrust      = (thrusting ? _thrust : 0);
		var thrustForce = SpaceShip.transform.up * thrust;
		SpaceShip.rb.AddForce(thrustForce, ForceMode.Force);
		
		thrustImage.SetActive(thrusting);

		// Rotation
		SpaceShip.rb.angularVelocity = turnRate * Mathf.Deg2Rad * Vector3.forward;

		// Wrap Ship Pos
		WrapPosition(SpaceShip.rb);
		
		// Clamp Velocity
		var vel = SpaceShip.rb.velocity;
		if (vel.magnitude > maxVelocity)
		{
			SpaceShip.rb.velocity = vel.normalized * maxVelocity;
		}
	}

	public void WrapPosition(Rigidbody rb)
	{

		if (rb.name == "Bullet(Clone)")
		{
			
		}
		// Wrap 
		var pos    = rb.position;
		var trPos  = screenTopRight.position;
		var blPos  = screenBottomLeft.position;
		var height = trPos.y - blPos.y;
		var width  = trPos.x - blPos.x;

		if (pos.x < blPos.x)
			pos.x += width;
		if (pos.x > trPos.x)
			pos.x -= width;
		
		if (pos.y < blPos.y)
			pos.y += height;
		if (pos.y > trPos.y)
			pos.y -= height;
		
		rb.position = pos;

	}
	
	void Update()
	{
		UpdateGame();
	}

	private void UpdateGame()
	{
		
	}
}

