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
	
	[FormerlySerializedAs("joinedLobbyUI")] [SerializeField] private DHTJoinedLobbyUI dhtJoinedLobbyUI;
	[SerializeField] private Transform     PlayerContentTransform;
	[SerializeField] private GameObject    PlayerSlotPrefab;

	[FormerlySerializedAs("LobbyListUI")] [SerializeField] private DHTLobbyListUI dhtLobbyListUI;
	[SerializeField] private GameObject  CreateLobbyUIGO;
	[SerializeField] private GameObject  JoinedLobbyUIGO;

	[SerializeField] private TMP_InputField gameModeTMP;


	private async void OnEnable()
	{
		DHTDebug.LogTag("Initializing Unity Services...", this);
		await UnityServices.InitializeAsync();
		AuthenticationService.Instance.SignedIn += () => { Debug.Log($"Signed In: {AuthenticationService.Instance.PlayerId}"); };
	}

	private async void Start()
	{
		//await FindFirstObjectByType<DHTSignInManager>().SignInAnonymously();
		//Init();
	}


	public void Init()
	{
		InitUI();
	}


	private void InitUI()
	{
		dhtLobbyListUI.gameObject.SetActive(true);
		CreateLobbyUIGO.SetActive(false);
		JoinedLobbyUIGO.SetActive(false);
		
		StartCoroutine(dhtLobbyListUI.StartUpdatingLobbyList());
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
				{ "Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) }
			}
		};
	}
}
