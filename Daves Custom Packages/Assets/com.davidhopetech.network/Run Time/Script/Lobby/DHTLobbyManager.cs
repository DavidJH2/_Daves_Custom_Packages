using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Utils;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;


public class DHTLobbyManager : MonoBehaviour
{
	
	[SerializeField] private DHTJoinedLobbyUI dhtJoinedLobbyUI;
	[SerializeField] private Transform     PlayerContentTransform;
	[SerializeField] private GameObject    PlayerSlotPrefab;

	[FormerlySerializedAs("LobbyListUI")] [SerializeField] private DHTLobbyListUI dhtLobbyListUI;
	[SerializeField] private GameObject  CreateLobbyUIGO;
	[SerializeField] private GameObject  JoinedLobbyUIGO;

	[SerializeField] private TMP_InputField gameModeTMP;

	public readonly string GameModeKey      = "GameMode";
	public readonly string RelayJoinCodeKey = "RelayJoinCode";
	public readonly string LobbyMapKey      = "LobbyMap";
	
	public readonly string PlayerNameKey      = "PlayerName";

	private async void OnEnable()
	{
		DHTDebug.LogTag("Initializing Unity Services...", this);
		await UnityServices.InitializeAsync();
		AuthenticationService.Instance.SignedIn += () => { Debug.Log($"Signed In: {AuthenticationService.Instance.PlayerId}"); };
		InitUI();
	}


	public void Init()
	{
	}


	private void InitUI()
	{
		dhtLobbyListUI.gameObject.SetActive(true);
		CreateLobbyUIGO.SetActive(false);
		JoinedLobbyUIGO.SetActive(false);
	}


	public void CreateLobbyButtonClicked()
	{
		CreateLobbyUIGO.SetActive(true);
	}


	public async Task<Player> GetPlayer()
	{
		string PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
		return new Player
		{
			Data = new Dictionary<string, PlayerDataObject>
			{
				{ PlayerNameKey, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) }
			}
		};
	}
}
