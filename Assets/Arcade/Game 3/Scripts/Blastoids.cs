using System;
using Arcade.Game_3.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class Blastoids : MonoBehaviour
{
	internal                 SpaceShip SpaceShip;
	[SerializeField] private float     _turnThreshold;
	[SerializeField] private float     _turnRate;
	[SerializeField] private float     _thrust;

	[SerializeField] private Transform ScreenTopRight;
	[SerializeField] private Transform ScreenBottomLeft;
	
	[SerializeField] private DTHJoystick dthJoystick;
	[SerializeField] private DTHButton   _thrustButton;

	private float turnRate;

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

		if (arg1 > _turnThreshold)
			turnRate  = -_turnRate;
		if (arg1 < -_turnThreshold)
			turnRate = _turnRate;
	}

	private void FixedUpdate()
	{
		UpdateShip();
	}

	private void UpdateShip()
	{
		var thrust      = (_thrustButton.pressed ? _thrust : 0);
		var thrustForce = SpaceShip.transform.up * thrust;

		DHTServiceLocator.dhtEventService.dhtUpdateDebugTeleportEvent.Invoke($"Thrust Force: {thrustForce}"); // Should not be TeleportEvent

		SpaceShip.rb.angularVelocity = turnRate * Mathf.Deg2Rad * Vector3.forward;
		SpaceShip.rb.AddForce(thrustForce, ForceMode.Force);

		var pos   = SpaceShip.rb.position;
		var trPos = ScreenTopRight.position;
		var blPos = ScreenBottomLeft.position;
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

