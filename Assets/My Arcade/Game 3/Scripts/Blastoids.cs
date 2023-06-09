using System;
using System.Globalization;
using Arcade.Game_3.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Extensions;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

// using Random = Unity.Mathematics.Random;

public class Blastoids : MonoBehaviour
{
	[SerializeField] internal int        NumLives = 3;
	[SerializeField] internal GameObject LeaderboardGO;
	
	[SerializeField] internal TextMeshProUGUI[] leaderboardNames;
	[SerializeField] internal TextMeshProUGUI[] leaderboardScore;
	
	[SerializeField] private  LineRenderer[] livesModels;
	[SerializeField] private  int            NumRocks = 0;
	[SerializeField] private  GameObject     bulletPrefab;
	[SerializeField] private  GameObject     rockPrefab;
	[SerializeField] private  Transform      bulletStartPos;
	[SerializeField] private  float          bulletSpeed    = 5;
	[SerializeField] private  float          bulletLifeTime = 1;
	[SerializeField] private  GameObject     screenSpace;
	
	[SerializeField] private GameObject thrustImage;
	[SerializeField] private float      turnThreshold;
	[SerializeField] private float      _turnRate;
	[SerializeField] private float      _thrust;
	[SerializeField] private float      maxVelocity;

	[SerializeField] private Transform screenTopRight;
	[SerializeField] private Transform screenBottomLeft;
	
	[SerializeField] private DTHJoystick joystick;
	[SerializeField] private DTHButton   thrustButton;
	[SerializeField] private DTHButton   fireButton;
	[SerializeField] private DTHButton   startButton;

	[SerializeField] private GameObject      GameOverTMPGO;
	[SerializeField] private TextMeshProUGUI ScoreTMP;

	[SerializeField] private AudioSource ThrustSound;
	[SerializeField] private AudioSource WeaponSound;
	[SerializeField] private AudioSource ExplosionSound;

	[SerializeField] private Leaderboard _leaderboard;


	internal int       lives;
	internal SpaceShip SpaceShip;
	private  float     turnRate;
	private  bool      lastFireButtonIsPressed;
	private  Blink     _blink;
	private  float     respawnTimer;
	private  int       score;
	private  bool      thrusting;
	void Start()
	{
		joystick.JoyStickEvent += OnJoystick;
		_blink                 =  startButton.gameObject.GetComponent<Blink>();

		if (!SpaceShip)
		{
			SpaceShip = transform.parent.GetComponentInChildren<SpaceShip>();
		}

		InitializeGame();
	}

	private void InitializeGame()
	{
		respawnTimer = 0;
		DisableShip();
		InitializeRocks();
		InitializeShip();
		DisableShip();
		lives = 0;
		score = 0;
		UpdateLivesModels();
	}


	internal async void  InitLeaderboard()
	{
		var highScores = (await _leaderboard.GetScoresAsync()).Results;
		for(var i = 0; i<highScores.Count; i++)
		{
			var entry = highScores[i];
			leaderboardNames[i].text = entry.PlayerName;
			leaderboardScore[i].text = entry.Score.ToString(CultureInfo.InvariantCulture);
		}
	}
	
	private void UpdateLivesModels()
	{
		for (var i=0; i<NumLives; i++)
		{
			var model = livesModels[i];
			model.enabled = ((i+1)<=lives);
		}

		;
	}

	private void DisableShip()
	{
		SpaceShip.alive = false;
	}


	private void InitializeShip()
	{
		thrusting                         = false;
		SpaceShip.alive                   = true;
		SpaceShip.transform.localPosition = Vector3.zero;
		SpaceShip.rb.velocity             = Vector3.zero;
		SpaceShip.rb.angularVelocity      = 0;
		SpaceShip.rb.SetRotation(0);
		ThrustSound.enabled = false;
		UpdateThrust();
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


	internal void InitializeRocks()
	{
		var rocks = GameObject.FindObjectsOfType<Rock>();
		foreach (var rock in rocks)
		{
			Destroy(rock.gameObject);
		}
		
		var trPos = screenTopRight.localPosition;
		var blPos = screenBottomLeft.localPosition;
		
		for (var i = 0; i < NumRocks; i++)
		{
			var rx  = UnityEngine.Random.Range(blPos.x, trPos.x);
			var ry  = UnityEngine.Random.Range(blPos.y, trPos.y);
			var pos = new Vector3(rx,ry,0);
	
			CreateRock(pos, 2f, 1);
		}
	}


	internal void CreateRock(Vector3 pos, float size, int generation)
	{
		var rockGO    = Instantiate(rockPrefab, screenSpace.transform);
		var rock      = rockGO.GetComponent<Rock>();
		var collider  = rockGO.GetComponent<CircleCollider2D>();
		var rockModel = rockGO.GetComponent<DTHLineRenderer>();

		rock.generation = generation;
		rock.size       = size;
		collider.radius = size * 1.25f;
		
		rockGO.transform.localPosition = pos;
		var points    = rockModel.points;
		var count     = rockModel.points.Length;
		var radStep   = 360 / count * Mathf.Deg2Rad;

		var rads           = 0.0f;
		var angOffsetRange = 18 * Mathf.Deg2Rad;
		for (var j = 0; j < count; j++)
		{
			var angOff = UnityEngine.Random.Range(-angOffsetRange, angOffsetRange);
			var radius = UnityEngine.Random.Range(size/2, size);
				
			var x = Mathf.Sin(rads + angOff) * radius;
			var y = Mathf.Cos(rads + angOff) * radius;
			points[j] =  new Vector3(x, y, 0);
			rads      += radStep;
		}

		rockModel.points = points;
	}

	private void FixedUpdate()
	{
		if (SpaceShip.alive)
		{
			UpdateShip();
		}
		else
		{
			if (respawnTimer > 0)
			{
				respawnTimer -= Time.deltaTime;
				if (respawnTimer < 0)
				{
					respawnTimer    = 0;
					InitializeShip();
				}
				return;
			}
			if (startButton.isPressed)
			{
				StartGame();
			}
			else
			{
				_blink.enabled = true;
			}
		}
	}

	private void StartGame()
	{
		InitializeShip();
		_blink.TurnOff();
		_blink.enabled = false;
		
		lives          = NumLives;
		UpdateLivesModels();
		GameOverTMPGO.SetActive(false);
		InitializeRocks();
		score = 0;
		UpdateUI();
		LeaderboardGO.SetActive(false);
	}


	private void UpdateShip()
	{
		// Fire
		var fireButtonIsPressed = fireButton.isPressed;
		if (fireButtonIsPressed && !lastFireButtonIsPressed)
		{
			WeaponSound.Play();
			var newBulletGO = Instantiate(bulletPrefab, bulletStartPos.position, quaternion.identity);
			var newBullet   = newBulletGO.GetComponent<Bullet>();
			newBullet.time       = bulletLifeTime;
			newBullet.gameEngine = this;
			
			var rb  = newBulletGO.GetComponent<Rigidbody2D>();
			var pos = transform.position; 
			rb.velocity = SpaceShip.rb.velocity + SpaceShip.transform.forward2D() * bulletSpeed;
			// rb.velocity = SpaceShip.transform.forward2D() * bulletSpeed;
		}

		lastFireButtonIsPressed = fireButtonIsPressed;
		
		// Thrust
		GetThrust();
		UpdateThrust();

		// Rotation
		SpaceShip.rb.angularVelocity = turnRate;

		// Wrap Ship Pos
		WrapPosition(SpaceShip.rb);
		
		// Clamp Velocity
		var vel = SpaceShip.rb.velocity;
		if (vel.magnitude > maxVelocity)
		{
			SpaceShip.rb.velocity = vel.normalized * maxVelocity;
		}
	}

	internal void GetThrust()
	{
		thrusting   = thrustButton.isPressed;
	}
	

	internal void UpdateThrust()
	{
		var thrust      = (thrusting ? _thrust : 0);
		var thrustForce = SpaceShip.transform.up * thrust;
		SpaceShip.rb.AddForce(thrustForce);
		ThrustSound.enabled = thrusting;
		thrustImage.SetActive(thrusting);
	}

	public void WrapPosition(Rigidbody2D rb)
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

	public void PlayerCrashed()
	{
		ExplosionSound.Play();
		thrusting = false;
		UpdateThrust();
		lives--;
		UpdateLivesModels();
		if (lives > 0)
		{
			respawnTimer = 3;
		}
		else
		{
			GameOver();
		}
	}


	internal void GameOver()
	{
		GameOverTMPGO.SetActive(true);
		UpdateUI();
		
		_leaderboard.AddScore(score);
		InitLeaderboard();
		LeaderboardGO.SetActive(true);
	}

	void UpdateUI()
	{
		ScoreTMP.text =  $"Score: {score.ToString()}";
	}
	
	public void AddScore(int points)
	{
		score         += points;
		UpdateUI();
	}
}