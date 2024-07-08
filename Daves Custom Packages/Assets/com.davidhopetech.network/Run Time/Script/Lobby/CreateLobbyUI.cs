using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField LobbyNameTMPIF;
    [SerializeField] private TMP_InputField lobbyMaxPlayersTMPInputField;
    [SerializeField] private TMP_InputField LobbyGameModeTMPIF;
    [SerializeField] public Toggle LobbyAccessibilityToggle;

    [SerializeField] private GameObject LobbyListUIGO;
    [SerializeField] private GameObject JoinedLobbyUIGO;

    private void OnEnable()
    {
	    JoinedLobbyUIGO = FindFirstObjectByType<JoinedLobbyUI>(FindObjectsInactive.Include).gameObject;
	    LobbyListUIGO = FindFirstObjectByType<LobbyListUI>(FindObjectsInactive.Include).gameObject;
    }

    void Start()
    {
        
    }


	public async void CreateLobby()
	{
		string lobbyName          = LobbyNameTMPIF.text;
		string accessibility = LobbyGameModeTMPIF.text;
		int    maxPlayers    = int.Parse(lobbyMaxPlayersTMPInputField.text);
		
		await CreateLobby(lobbyName, accessibility, maxPlayers);
		gameObject.SetActive(false);
		JoinedLobbyUIGO.SetActive(true);
	}


	async public Task<Lobby> CreateLobby(string lobbyName, string accessibilityStr, int maxPlayers = 4)
	{
		Lobby lobby = null;
		try
		{
			bool accessibility = accessibilityStr == "Private";
			
			CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
			{
				IsPrivate = false,
				Player    = await DHTLobbyManager.GetPlayer(),
				Data = new Dictionary<string, DataObject>
				{
					{"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag")},
					{"Map", new DataObject(DataObject.VisibilityOptions.Public, "Open World")}
				}
			};

			lobby                     = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
			JoinedLobbyUI._hostLobby  = lobby;
			JoinedLobbyUI.JoinedLobby = JoinedLobbyUI._hostLobby;
			 //SubscribeLoobyChanges(DHTLobbyManager.JoinedLobby);
			Debug.Log($"Created Lobby: {lobby.Name} {lobby.MaxPlayers} {lobby.Id} {lobby.LobbyCode}");
			
			JoinedLobbyUIGO.gameObject.SetActive(true);
			LobbyListUIGO.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}

		return lobby;
	}
}
