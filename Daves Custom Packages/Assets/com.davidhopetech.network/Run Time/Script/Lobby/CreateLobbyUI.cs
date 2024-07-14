using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField LobbyNameTMPIF;
    [SerializeField] private TMP_InputField lobbyMaxPlayersTMPInputField;
    [SerializeField] private TMP_InputField LobbyGameModeTMPIF;
    [SerializeField] public Toggle LobbyAccessibilityToggle;

    [SerializeField] private GameObject LobbyListUIGO;
    [SerializeField] private GameObject JoinedLobbyUIGO;

    private DHTLobbyManager _lobbyManager;
    

    private void OnEnable()
    {
	    JoinedLobbyUIGO = FindFirstObjectByType<DHTJoinedLobbyUI>(FindObjectsInactive.Include).gameObject;
	    LobbyListUIGO = FindFirstObjectByType<DHTLobbyListUI>(FindObjectsInactive.Include).gameObject;
    }

    void Start()
    {
	    _lobbyManager = FindFirstObjectByType<DHTLobbyManager>(FindObjectsInactive.Include);
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
				Player    = await _lobbyManager.GetPlayer(),
				Data = new Dictionary<string, DataObject>
				{
					{_lobbyManager.GameModeKey, new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag")},
					{_lobbyManager.LobbyMapKey, new DataObject(DataObject.VisibilityOptions.Public, "Open World")},
					{_lobbyManager.RelayJoinCodeKey, new DataObject(DataObject.VisibilityOptions.Member, "")}
				}
			};

			lobby                     = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
			DHTJoinedLobbyUI._hostLobby  = lobby;
			DHTJoinedLobbyUI.JoinedLobby = DHTJoinedLobbyUI._hostLobby;
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
