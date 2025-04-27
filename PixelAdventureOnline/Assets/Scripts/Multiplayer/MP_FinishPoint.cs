using UnityEngine;
using Fusion;
using System.Collections.Generic;
using System.Linq;

public class MP_FinishPoint : NetworkBehaviour, IPlayerLeft
{
    private Animator anim;

    [Networked]
    private NetworkDictionary<PlayerRef, NetworkBool> PlayersFinished => default;

    private int totalPlayers;
    private bool hasProcessedResults;

    public override void Spawned()
    {
        anim = GetComponent<Animator>();
        hasProcessedResults = false;

        if (Object.HasStateAuthority)
        {
            totalPlayers = Runner.ActivePlayers.Count();
            Debug.Log($"[Fusion] Total players in room: {totalPlayers}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Object.HasStateAuthority) return;

        MP_Player player = collision.GetComponent<MP_Player>();

        if (player != null)
        {
            RPC_PlayerFinished(player.Object.InputAuthority);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_PlayerFinished(PlayerRef player)
    {
        if (!Object.HasStateAuthority) return;

        if (!PlayersFinished.ContainsKey(player))
        {
            PlayersFinished.Add(player, true);

            Debug.Log($"[Fusion] Player {player.PlayerId} finished. Total finished: {PlayersFinished.Count}/{totalPlayers}");

            RPC_TriggerFinishAnimation();

            CheckAllPlayersFinished();
        }
    }

    private void CheckAllPlayersFinished()
    {
        if (!Object.HasStateAuthority || hasProcessedResults) return;

        if (PlayersFinished.Count >= totalPlayers)
        {
            hasProcessedResults = true;

            Debug.Log("[Fusion] All players have finished!");

            RPC_GameFinished();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_TriggerFinishAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("active");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_GameFinished()
    {
        if (Object.HasInputAuthority)
        {
            float finishTime = (float)Runner.SimulationTime;
            SaveResults(finishTime);
        }
    }

    private void SaveResults(float finishTime)
    {
        Debug.Log($"[Fusion] Saving results for local player. Finish Time: {finishTime}");
        // TODO: Gửi dữ liệu finishTime lên Firebase hoặc lưu cục bộ
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!Object.HasStateAuthority) return;

        if (PlayersFinished.ContainsKey(player))
        {
            PlayersFinished.Remove(player);
        }

        totalPlayers = Runner.ActivePlayers.Count();
        CheckAllPlayersFinished();
    }
}