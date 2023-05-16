using System;
using Arcade.Game_3.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class Blastoids : MonoBehaviour
{
	[SerializeField] private GameObject thrustImage;
	[SerializeField] private float      turnThreshold;
	[SerializeField] private float      _turnRate;
	[SerializeField] private float      _thrust;
	[SerializeField] private float      maxVelocity;

	[SerializeField] private Transform screenTopRight;
	[SerializeField] private Transform screenBottomLeft;
	
	[SerializeField] private DTHJoystick dthJoystick;
	[SerializeField] private DTHButton   thrustButton;

	internal SpaceShip SpaceShip;
	private  float     turnRate;

	void Start()
	{
		dthJoystick.JoyStickEvent += OnDthJoystick;

		if (!SpaceShip)
		{
			SpaceShip = transform.parent.GetComponentInChildren<SpaceShip>();
		}
	}

	private void OnDthJoystick(float arg1, float arg2)
	{
		turnRate = 0.0f;

		if (arg1 > turnThreshold)
			turnRate  = -_turnRate;
		if (arg1 < -turnThreshold)
			turnRate = _turnRate;
	}

	private void FixedUpdate()
	{
		UpdateShip();
	}

	private void UpdateShip()
	{
		var thrusting   = thrustButton.pressed;
		var thrust      = (thrusting ? _thrust : 0);
		var thrustForce = SpaceShip.transform.up * thrust;
		
		thrustImage.SetActive(thrusting);

		// DHTServiceLocator.dhtEventService.dhtUpdateDebugTeleportEvent.Invoke($"Thrust Force: {thrustForce}"); // Should not be TeleportEvent

		SpaceShip.rb.angularVelocity = turnRate * Mathf.Deg2Rad * Vector3.forward;
		SpaceShip.rb.AddForce(thrustForce, ForceMode.Force);

		var pos   = SpaceShip.rb.position;
		var trPos = screenTopRight.position;
		var blPos = screenBottomLeft.position;
		var height = trPos.y - blPos.y;
		var width = trPos.x - blPos.x;

		// Wrap X
		if (pos.x < blPos.x)
			pos.x += width;
		if (pos.x > trPos.x)
			pos.x -= width;
		
		// Wrap Y
		if (pos.y < blPos.y)
			pos.y += height;
		if (pos.y > trPos.y)
			pos.y -= height;

		// Clamp Velocity
		var vel = SpaceShip.rb.velocity;
		if (vel.magnitude > maxVelocity)
		{
			SpaceShip.rb.velocity = vel.normalized * maxVelocity;
		}

		SpaceShip.rb.position = pos;
	}

	void Update()
	{
		UpdateGame();
	}

	private void UpdateGame()
	{
		
	}
}

