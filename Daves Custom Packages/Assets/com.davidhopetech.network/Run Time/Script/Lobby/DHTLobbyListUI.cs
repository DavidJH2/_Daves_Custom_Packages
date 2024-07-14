using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Utils;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class DHTLobbyListUI : MonoBehaviour
{
	[SerializeField] private Transform      LobbyListContentTransform;
	[SerializeField] private GameObject     LobbySlotPrefab;
	[SerializeField] private TMP_InputField lobbyCodeTMP;
	
	private DHTLobbyManager _lobbyManager;

	void Start()
    {
	    _lobbyManager                           =  FindFirstObjectByType<DHTLobbyManager>(FindObjectsInactive.Include);
	    AuthenticationService.Instance.SignedIn += Init;
    }

	void Init()
	{
		// Debug.Log("DHTLobbyList.Init()");
		
	    StartCoroutine(StartUpdatingLobbyList());
	}

	public IEnumerator StartUpdatingLobbyList()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(1.2f);
			//Debug.Log($"Updating Lobby List: {UpdatingLobbyList}");
			UpdateLobbyList();
		}
	}


	async void UpdateLobbyList()
	{
		if (DHTJoinedLobbyUI._joinedLobby == null)
		{
			QueryResponse response = await FindLobbies();
			if (response == null)
			{
				return;
			}
			
			Lobby[] lobbies = response.Results.ToArray();
			//Debug.Log($"Lobby Count: {lobbies.Length}");


			int childCount      = LobbyListContentTransform.childCount;
			
			int lobbyEntryCountDelta = lobbies.Length - childCount;
			if ( lobbyEntryCountDelta>0)								// Create needed slots
			{
				for (int i = 0; i < lobbyEntryCountDelta; i++)
				{
					Instantiate(LobbySlotPrefab, LobbyListContentTransform);
				}
			}
			else
			{
				if (lobbyEntryCountDelta < 0)							// Deactivate unneeded slots
				{
					for (int i = childCount - (-lobbyEntryCountDelta); i < childCount; i++)
					{
						LobbyListContentTransform.GetChild(i).gameObject.SetActive(false);
					}
				}
			}
			
			int activeChildreen = 0;									// Count active slots
			for (int i = 0; i < childCount; i++)
			{
				if (LobbyListContentTransform.GetChild(i).gameObject.activeSelf)
				{
					activeChildreen++;
				}
			}

			int delta = lobbies.Length - activeChildreen;					// Reactivate needed slots
			if (delta>0)
			{
				for (int i = activeChildreen; i < childCount; i++)
				{
					LobbyListContentTransform.GetChild(i).gameObject.SetActive(true);
				}
			}

			for (int i = 0; i < lobbies.Length; i++)
			{
				Lobby      lobby         = lobbies[i];
				GameObject slotGO        = LobbyListContentTransform.GetChild(i).gameObject;
				Transform  slotTransform = slotGO.transform;

				slotTransform.GetChild(0).GetComponent<TMP_Text>().text = lobby.Name;
				slotTransform.GetChild(1).GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
				slotTransform.GetChild(2).GetComponent<TMP_Text>().text = $"{lobby.Data[_lobbyManager.GameModeKey].Value}";
			}
		}
	}


	async public void ListLobbies()
	{
		QueryResponse response = await FindLobbies();
		if (response != null)
		{
			Lobby[] lobbies = response.Results.ToArray();

			Debug.Log($"Lobbies found: {response.Results.Count}");
			foreach (var lobby in lobbies)
			{
				Debug.Log($"Name: {lobby.Name}   Players: {lobby.Players.Count}/{lobby.MaxPlayers}  {lobby.Data[_lobbyManager.GameModeKey].Value}  {lobby.Data[_lobbyManager.LobbyMapKey].Value}");
			}
		}
		else
		{
			Debug.Log("No Lobbies found");
		}
	}


	async public Task<QueryResponse> FindLobbies()
	{
		// Debug.Log("Finding Lobbies");
		
		try
		{
			QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
			{
				Count = 25,
				Filters = new List<QueryFilter>
				{
					new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
					// new QueryFilter(QueryFilter.FieldOptions.S1, "CaptureTheFlag", QueryFilter.OpOptions.EQ)
				},
				Order = new List<QueryOrder>
				{
					new QueryOrder(false, QueryOrder.FieldOptions.Created)
				}
			};

			QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

			return response;
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
			return null;
		}
	}


	async public void JoinLobbyByCode()
	{
		if (lobbyCodeTMP.text == "")
		{
			Debug.Log("No Lobby Code");
			return;
		}

		JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
		{
			Player = await _lobbyManager.GetPlayer()
		};

		try
		{
			DHTJoinedLobbyUI._joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCodeTMP.text, joinLobbyByCodeOptions);
			
			FindFirstObjectByType<DHTJoinedLobbyUI>().gameObject.SetActive(true);
			gameObject.SetActive(false);
			// SubscribeLoobyChanges(_joinedLobby);
		}
		catch(LobbyServiceException)
		{}
	}


	public async void QuickJoin()
	{
		try
		{
			QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
			{
				Player = await _lobbyManager.GetPlayer()
			};
			
			DHTJoinedLobbyUI._joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
			
			DHTJoinedLobbyUI joinedLobbyUI =  FindFirstObjectByType<DHTJoinedLobbyUI>(FindObjectsInactive.Include);
			DHTDebug.Log($"UI: {joinedLobbyUI.name}");
			joinedLobbyUI.gameObject.SetActive(true);
			gameObject.SetActive(false);
			// SubscribeLoobyChanges(_joinedLobby);
		}
		catch (LobbyServiceException e)
		{
			Debug.Log($"{e}");
		}
	}
}
