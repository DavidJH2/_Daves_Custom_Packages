using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

public class DHTJoinedLobbyUI : MonoBehaviour
{
	[SerializeField] private TMP_Text       NameTMP;
	[SerializeField] private TMP_Text       PlayerCountTMP;
	[SerializeField] private TMP_Text       GameModeTMP;
	[SerializeField] private TMP_InputField GameModeTMPIF;
	
	[SerializeField] private GameObject LobbyCodeGroupGO;
	[SerializeField] private TMP_Text   lobbyCodeTMPText;
	[SerializeField] private GameObject LobbyListUIGO;
	[SerializeField] private Transform  PlayerListContentTransform;
	[SerializeField] private GameObject PlayerSlotPrefab;
	[SerializeField] private GameObject StartButtonGO;


	private DHTLobbyManager _lobbyManager;
	private RelayManager _relayManager;
	
	public static            Lobby      JoinedLobby;
	Lobby                               _lobby;
	internal static Lobby               _hostLobby;


	public Lobby lobby
	{
		get => _lobby;
		set
		{
			_lobby              = value;
			NameTMP.text        = lobby.Name;
			PlayerCountTMP.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
			GameModeTMP.text    = lobby.Data[_lobbyManager.GameModeKey].Value;
		}
	}

	
	void Start()
	{
		_lobbyManager = FindFirstObjectByType<DHTLobbyManager>(FindObjectsInactive.Include);
		_relayManager = FindFirstObjectByType<RelayManager>(FindObjectsInactive.Include);
		StartButtonGO.SetActive(_hostLobby!=null);
		SubscribeLobbyChanges(JoinedLobby);

		if (_hostLobby != null)
		{
			InvokeRepeating(nameof(LobbyHeartBeat), 10, 10);
			LobbyCodeGroupGO.SetActive(true);
			lobbyCodeTMPText.text = _hostLobby.LobbyCode;
		}

		UpdateLobbyUI();
		
		lobby = JoinedLobby;
	}


	public async void StartButtonPressed()
	{
		string lobbyCode = await _relayManager.CreateRelay(JoinedLobby.MaxPlayers);
		ModifyLobby("", lobbyCode);
	}
	
	
	async void LobbyHeartBeat()
	{
		if (_hostLobby != null)
		{
			try
			{
				await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
			}
			catch (LobbyServiceException e)
			{
				Debug.Log(e);
			}
		}
	}


	
	LobbyEventCallbacks _callbacks = new LobbyEventCallbacks();
	private async void SubscribeLobbyChanges(Lobby lobby)
	{
		// SUBSCRIBE TO CALLS
		_callbacks.LobbyChanged += OnRoomPropertiesUpdate;
		try
		{
			var _lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobby.Id, _callbacks);
			Debug.Log(_lobbyEvents);
			Debug.Log($"Subscribe... {lobby.Id}");
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}
	}


	void OnRoomPropertiesUpdate(ILobbyChanges changes)
	{
		UpdateLobbyUI();
	}

	async void UpdateLobbyUI()
	{
		try
		{
			JoinedLobby = (await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id));
			
			Player[] players = JoinedLobby.Players.ToArray();
			//Debug.Log($"Lobby Count: {lobbies.Length}");


			int childCount      = PlayerListContentTransform.childCount;
			
			int lobbyEntryCountDelta = players.Length - childCount;
			if ( lobbyEntryCountDelta>0)								// Create needed slots
			{
				for (int i = 0; i < lobbyEntryCountDelta; i++)
				{
					Instantiate(PlayerSlotPrefab, PlayerListContentTransform);
				}
			}
			else
			{
				if (lobbyEntryCountDelta < 0)							// Deactivate unneeded slots
				{
					for (int i = childCount - (-lobbyEntryCountDelta); i < childCount; i++)
					{
						PlayerListContentTransform.GetChild(i).gameObject.SetActive(false);
					}
				}
			}
			
			int activeChildreen = 0;									// Count active slots
			for (int i = 0; i < childCount; i++)
			{
				if (PlayerListContentTransform.GetChild(i).gameObject.activeSelf)
				{
					activeChildreen++;
				}
			}

			int delta = players.Length - activeChildreen;					// Reactivate needed slots
			if (delta>0)
			{
				for (int i = activeChildreen; i < childCount; i++)
				{
					PlayerListContentTransform.GetChild(i).gameObject.SetActive(true);
				}
			}

			for (int i = 0; i < players.Length; i++)
			{
				var        player             = players[i];
				//var        player        = players[i];
				GameObject slotGO        = PlayerListContentTransform.GetChild(i).gameObject;
				Transform  slotTransform = slotGO.transform;
				
				string playerName =	player.Data[_lobbyManager.PlayerNameKey].Value;
				TMP_Text nameTMP = slotTransform.GetChild(0).GetComponent<TMP_Text>();
				nameTMP.text = playerName;
			}
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}
	}


	public void ModifyLobby()
	{
		string newMap = GameModeTMPIF.text;
		ModifyLobby(newMap);
	}
	
	public async void ModifyLobby(string newGameMode, string lobbyCode = "")
	{
		if (_hostLobby != null)
		{
			try
			{
				newGameMode = newGameMode != "" ? newGameMode : _hostLobby.Data[_lobbyManager.GameModeKey].Value;
				Dictionary<string, DataObject> data = new Dictionary<string, DataObject>
				{
					{ _lobbyManager.GameModeKey, new DataObject(DataObject.VisibilityOptions.Public, newGameMode) }
				};

				if (lobbyCode != "")
				{
					data[_lobbyManager.RelayJoinCodeKey] = new DataObject(DataObject.VisibilityOptions.Member, lobbyCode);
				}
				
				UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions
				{
					Data = data
				};
				_hostLobby   = await LobbyService.Instance.UpdateLobbyAsync(_hostLobby.Id, updateLobbyOptions);
				JoinedLobby = _hostLobby;
			}
			catch (LobbyServiceException)
			{
				Debug.Log("Couldn't Update Lobby");
			}
		}
		else
		{
			Debug.Log("No Host Lobby");
		}
	}

	public void LeaveLobbyButtonClicked()
	{
		LeaveLobby();
		
		gameObject.SetActive(false);
		LobbyListUIGO.SetActive(true);
	}

	private async void LeaveLobby()
	{
		if (JoinedLobby != null)
		{
			await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
			JoinedLobby = null;
			_hostLobby  = null;
		}
	}

	private void OnDestroy()
	{
		LeaveLobby();
	}
}
