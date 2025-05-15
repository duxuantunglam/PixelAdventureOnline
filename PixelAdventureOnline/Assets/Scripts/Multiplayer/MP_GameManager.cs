using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class MP_GameManager : NetworkBehaviour, IPlayerLeft
{
    public static MP_GameManager instance { get; private set; }

    public static event Action<PlayerRef, float> OnPlayerFinished;
    public static event Action OnAllPlayersFinished;
    public static event Action OnGameReset;

    [Networked]
    private NetworkDictionary<PlayerRef, float> finishTimes => default;

    private int totalPlayers;
    private bool hasGameFinished = false;

    public override void Spawned()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (Object.HasStateAuthority && Runner != null)
        {
            totalPlayers = Runner.ActivePlayers.Count();
            Debug.Log($"[Fusion][GameManager] Total players at start: {totalPlayers}");
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SaveFinishTime(PlayerRef player, float time)
    {
        if (!Object.HasStateAuthority) return;
        if (time < 0) return;

        if (!finishTimes.ContainsKey(player))
        {
            finishTimes.Add(player, time);
            Debug.Log($"[Fusion][GameManager] Player {player.PlayerId} finished with time {time}");

            RPC_NotifyPlayerFinished(player, time);

            CheckAllPlayersFinished();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_NotifyPlayerFinished(PlayerRef player, float time)
    {
        OnPlayerFinished?.Invoke(player, time);
    }

    private void CheckAllPlayersFinished()
    {
        if (!Object.HasStateAuthority || hasGameFinished) return;

        if (finishTimes.Count >= totalPlayers)
        {
            hasGameFinished = true;
            Debug.Log("[Fusion][GameManager] All players have finished!");

            ProcessGameFinished();
        }
    }

    private void ProcessGameFinished()
    {
        if (!Object.HasStateAuthority) return;

        RPC_NotifyGameFinished();

        var sortedResults = GetSortedResults();
        foreach (var entry in sortedResults)
        {
            Debug.Log($"Player {entry.Key.PlayerId} => Finish Time: {entry.Value}");
        }

        // TODO: Đẩy dữ liệu lên Firebase/Database
        // TODO: Chuyển scene sau một khoảng thời gian
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_NotifyGameFinished()
    {
        OnAllPlayersFinished?.Invoke();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!Object.HasStateAuthority) return;

        if (finishTimes.ContainsKey(player))
        {
            finishTimes.Remove(player);
        }

        totalPlayers = Runner.ActivePlayers.Count();

        if (totalPlayers == 0)
        {
            ResetGame();
        }
        else
        {
            CheckAllPlayersFinished();
        }
    }

    public float GetFinishTime(PlayerRef player)
    {
        if (finishTimes.TryGet(player, out float time))
        {
            return time;
        }
        return -1f;
    }

    public List<KeyValuePair<PlayerRef, float>> GetSortedResults()
    {
        if (finishTimes.Count == 0)
            return new List<KeyValuePair<PlayerRef, float>>();

        return finishTimes.OrderBy(pair => pair.Value).ToList();
    }

    public void ResetGame()
    {
        if (!Object.HasStateAuthority) return;

        finishTimes.Clear();
        hasGameFinished = false;
        totalPlayers = Runner.ActivePlayers.Count();

        RPC_NotifyGameReset();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_NotifyGameReset()
    {
        OnGameReset?.Invoke();
    }
}