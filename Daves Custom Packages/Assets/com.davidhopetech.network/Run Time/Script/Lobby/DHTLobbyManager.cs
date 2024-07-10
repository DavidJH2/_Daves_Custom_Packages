using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;


public class DHTLobbyManager : MonoBehaviour
{
	
	[SerializeField] private JoinedLobbyUI joinedLobbyUI;
	[SerializeField] private Transform     PlayerContentTransform;
	[SerializeField] private GameObject    PlayerSlotPrefab;

	[SerializeField] private LobbyListUI LobbyListUI;
	[SerializeField] private GameObject  CreateLobbyUIGO;
	[SerializeField] private GameObject  JoinedLobbyUIGO;

	[SerializeField] private TMP_InputField gameModeTMP;


	private async void OnEnable()
	{
		await UnityServices.InitializeAsync();
		AuthenticationService.Instance.SignedIn += () => { Debug.Log($"Signed In: {AuthenticationService.Instance.PlayerId}"); };
	}

	private void Start()
	{
		Init();
	}


	public async void Init()
	{
		InitUI();
	}


	private void InitUI()
	{
		LobbyListUI.gameObject.SetActive(true);
		CreateLobbyUIGO.SetActive(false);
		JoinedLobbyUIGO.SetActive(false);
		
		StartCoroutine(LobbyListUI.StartUpdatingLobbyList());
	}


	public void CreateLobbyButtonClicked()
	{
		CreateLobbyUIGO.SetActive(true);
	}


	static public async Task<Player> GetPlayer()
	{
		string PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
		return new Player
		{
			Data = new Dictionary<string, PlayerDataObject>
			{
				{ "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) }
			}
		};
	}
}
