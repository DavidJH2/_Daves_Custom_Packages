using System;
using System.Globalization;
using System.Collections;
using System.Linq;
using System.Security.Authentication;
using Arcade.Game_3.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Extensions;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// using Random = Unity.Mathematics.Random;

public class Blastoids : MonoBehaviour
{
	[SerializeField] internal int        NumLives = 3;
	[SerializeField] internal GameObject LeaderboardGO;

	[SerializeField] internal float               MinRockSpawnDist = .5f;
	[SerializeField] internal InputActionProperty LeftMenuAction;

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
	
	[SerializeField] private DHTJoystick joystick;
	[SerializeField] private DHTButton   thrustButton;
	[SerializeField] private DHTButton   fireButton;
	[SerializeField] private DHTButton   startButton;

	[SerializeField] private GameObject      GameOverTMPGO;
	[SerializeField] private TextMeshProUGUI ScoreTMP;

	[SerializeField] private AudioSource ThrustSound;
	[SerializeField] private AudioSource WeaponSound;
	[SerializeField] private AudioSource ExplosionSound;

	[SerializeField] private Leaderboard _leaderboard;

	[SerializeField]         TMP_InputField PlayerNameInputField;
	[SerializeField] private GameObject     PlayerHudMenu;
	[SerializeField] private GameObject     warningMessagePanel;
	[SerializeField] private TMP_Text       exceptionScreenTMP;
	// private                  GameObject     nullGO = null;

	
	private TMP_Text warningMessageTMP;

	internal int       livesLeft;
	internal SpaceShip SpaceShip;
	private  float     turnRate;
	private  bool      lastFireButtonIsPressed;
	private  Blink     _blink;
	private  float     respawnTimer;
	private  int       score;
	private  bool      thrusting;
	
	void OnEnable()
	{
		Application.logMessageReceived += LogCallback;
	}

	private void LogCallback(string condition, string stacktrace, LogType type)
	{
		if (type != LogType.Log)
		{
			exceptionScreenTMP.text += $"Log Type:\n{type}\n\nCondition:\n{condition}\n\nStack Trace:\n{stacktrace}";
		}
	}

	void Start()
	{
		warningMessageTMP = warningMessagePanel.GetComponentInChildren<TMP_Text>();
		LeftMenuAction.action.Enable();
		joystick.JoyStickEvent         += OnJoystick;
		_blink                         =  startButton.gameObject.GetComponent<Blink>();

		if (!SpaceShip)
		{
			SpaceShip = transform.parent.GetComponentInChildren<SpaceShip>();
		}

		InitializeGame();
		GameOverTMPGO.SetActive(false);
	}

	private void InitializeGame()
	{
		respawnTimer = 0;
		DisableShip();
		InitializeRocks();
		InitializeShip();
		DisableShip();
		livesLeft = 0;
		score = 0;
		UpdateLivesModels();
	}


	internal void  UpdateLeaderboard()
	{
		UpdateHighScores();
		UpdateUserNameUI();
	}


	private string PlayName;


	public async void SetPlayerName()
	{
		var newName = PlayerNameInputField.text;

		if (newName == "" || newName.IndexOf(" ", StringComparison.Ordinal)!=-1)
		{
			warningMessagePanel.SetActive(true);
			warningMessageTMP.text = "Player Name can not be empty or contain spaces";
		}
		else
		{
			try
			{
				PlayerNameInputField.text = await _leaderboard.SetPlayerName(newName);
				warningMessagePanel.SetActive(false);
				UpdateLeaderboard();
			}
			catch (Exception e)
			{
				var message = e.Message;
				// exceptionScreenTMP.text = e.ToString();

				if (e is RequestFailedException && message.IndexOf("Too Many Requests", StringComparison.Ordinal)!=-1)
				{
					warningMessagePanel.SetActive(true);
					warningMessageTMP.text = "Name Changing Too Frequently";
				}
				else
				{
					throw e;
				}
					/*
					else
					{
						warningMessagePanel.SetActive(false);
						Console.WriteLine(e);
						throw;
					}
					*/
			}
		}
	}
	
	
	internal async void UpdateUserNameUI()
	{
		PlayName                  = await _leaderboard.GetPlayerName();
		PlayerNameInputField.text = PlayName;
	}

	
	internal async void UpdateHighScores()
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
			model.enabled = ((i+1)<=livesLeft);
		}

		;
	}

	private void DisableShip()
	{
		SpaceShip.alive = false;
	}


	private void AttemptToRespawnShip()
	{
		var rocksLeft = FindObjectsOfType<Rock>();

		foreach (var rock in rocksLeft)
		{
			var dist = (rock.transform.position - SpaceShip.transform.position).magnitude;
			Debug.Log($"Rock Dist: {dist}");
			if (dist < MinRockSpawnDist)
			{
				return;
			}
		}
		
		SpaceShip.alive = true;
	}

	private void InitializeShip()
	{
		thrusting                         = false;
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
		Vector3 pos;
		var rocks = GameObject.FindObjectsOfType<Rock>();
		foreach (var rock in rocks)
		{
			Destroy(rock.gameObject);
		}
		
		var trPos = screenTopRight.localPosition;
		var blPos = screenBottomLeft.localPosition;
		
		for (var i = 0; i < NumRocks; i++)
		{
			do
			{
				var rx  = UnityEngine.Random.Range(blPos.x, trPos.x);
				var ry  = UnityEngine.Random.Range(blPos.y, trPos.y);
				pos = new Vector3(rx, ry, 0);
				
				
			} while ((pos - SpaceShip.transform.localPosition).magnitude<MinRockSpawnDist);

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
		collider.radius = size + .1f;
		
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
					respawnTimer = 0;
					InitializeShip();
				}
				else
				{
					return;
				}
			}

			if (livesLeft > 0)
			{
				AttemptToRespawnShip();
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
		if (_switchToHighScoreAfterDelay != null)
		{
			StopCoroutine(_switchToHighScoreAfterDelay);
		}

		InitializeShip();
		_blink.TurnOff();
		_blink.enabled = false;
		
		livesLeft          = NumLives;
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
		if (LeftMenuAction.action.WasPerformedThisFrame())
		{
			PlayerHudMenu.SetActive(!PlayerHudMenu.activeSelf);
		}
	}

	public void PlayerCrashed()
	{
		ExplosionSound.Play();
		thrusting = false;
		UpdateThrust();
		livesLeft--;
		UpdateLivesModels();
		if (livesLeft > 0)
		{
			respawnTimer = 3;
		}
		else
		{
			GameOver();
		}
	}

	private Coroutine _switchToHighScoreAfterDelay;

	internal void GameOver()
	{
		GameOverTMPGO.SetActive(true);
		UpdateUI();
		_leaderboard.AddScore(score);

		_switchToHighScoreAfterDelay = StartCoroutine(SwitchToHighScoreAfterDelay());
	}
	
	IEnumerator SwitchToHighScoreAfterDelay()
	{
		yield return new WaitForSeconds(2);

		if (!SpaceShip.alive)
		{
			GameOverTMPGO.SetActive(false);
			UpdateLeaderboard();
			LeaderboardGO.SetActive(true);
		}
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

	internal void CheckRoundComplete()
	{
		StartCoroutine(CheckRoundCompleteNextFrame());
	}

	IEnumerator CheckRoundCompleteNextFrame()
	{
		yield return 0;
		
		var rocksLeft = FindObjectsOfType<Rock>();
		if (rocksLeft.Length == 0 && livesLeft > 0)
		{
			yield return new WaitForSeconds(1);
			NumRocks++;
			InitializeRocks();
		}
	}
}