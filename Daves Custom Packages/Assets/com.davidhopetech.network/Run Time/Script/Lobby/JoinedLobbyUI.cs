using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

public class JoinedLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text       NameTMP;
    [SerializeField] private TMP_Text       PlayerCountTMP;
    [SerializeField] private TMP_Text       GameModeTMP;
    [SerializeField] private TMP_InputField GameModeTMPIF;
    
    [SerializeField] private GameObject LobbyCodeGroupGO;
    [SerializeField] private TMP_Text   lobbyCodeTMPText;
    [SerializeField] private GameObject LobbyListUIGO;

    public static Lobby   JoinedLobby;
    Lobby                 _lobby;
    internal static Lobby _hostLobby;

    private void Awake()
    {
	    gameObject.SetActive(false);
	    LobbyListUIGO = FindFirstObjectByType<LobbyListUI>(FindObjectsInactive.Include).gameObject;
    }

    public Lobby lobby
    {
        get => _lobby;
        set
        {
            _lobby              = value;
            NameTMP.text        = lobby.Name;
            PlayerCountTMP.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
            GameModeTMP.text    = lobby.Data["GameMode"].Value;
        }
    }

    
    void Start()
    {
        InvokeRepeating(nameof(LobbyHeartBeat), 10, 10);
        SubscribeLobbyChanges(JoinedLobby);

        if (_hostLobby != null)
        {
	        LobbyCodeGroupGO.SetActive(true);
	        lobbyCodeTMPText.text = _hostLobby.LobbyCode;
        }

        lobby = JoinedLobby;
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
	
	
	async void OnRoomPropertiesUpdate(ILobbyChanges changes)
	{
		try
		{
			JoinedLobby = (await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id));
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}
	}


	public void UpdateLobby()
	{
		string newMap = GameModeTMPIF.text;
		UpdateLobby(newMap);
	}
	
	public async void UpdateLobby(string newGameMode)
	{
		if (_hostLobby != null)
		{
			try
			{
				UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions
				{
					Data = new Dictionary<string, DataObject>
					{
						{ "GameMode", new DataObject(DataObject.VisibilityOptions.Public, newGameMode) }
					}
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

	public async void RemovePlayer()
	{
		await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
		JoinedLobby = null;
		_hostLobby   = null;
		
		gameObject.SetActive(false);
		LobbyListUIGO.SetActive(true);
	}
}
