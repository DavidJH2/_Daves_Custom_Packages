using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards.Models;


public class Leaderboard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Dashboard
    [SerializeField] private Blastoids gameEngine;
    
    const   string    LeaderboardId = "Test";

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    async void Awake()
    {
        gameEngine = FindObjectOfType<Blastoids>();
        await UnityServices.InitializeAsync();

        await SignInAnonymously();
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            gameEngine.UpdateLeaderboard();
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void AddScore(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async Task<LeaderboardScoresPage> GetScoresAsync()
    {
        LeaderboardScoresPage scoresResponse;
        
        try
        {
            scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            scoresResponse = new LeaderboardScoresPage();
        }

        return scoresResponse;
    }

    public async void GetPaginatedScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions{Offset = Offset, Limit = Limit});
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse = 
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetVersionScores()
    {
        var versionScoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
    Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
    }


    public async Task<string> SetPlayerName(string newName)
    {
        Debug.Log("*****  Updating Player  *****");
        var updatedPlayerName = await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
        return updatedPlayerName;
    }
    
    
    public async Task<String> GetPlayerName()
    {
        string playerName = "";
        
        try
        {
            playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log($"Player Name: {playerName}");

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return playerName;
    }
}
