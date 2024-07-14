using System;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public async Task<string> CreateRelay(int maxPlayers)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers-1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            Debug.Log($"Relay Join Code: {joinCode}");
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return "";
        }
    }

    public async void joinRelay(string joinCode)
    {
        try
        {
            await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
