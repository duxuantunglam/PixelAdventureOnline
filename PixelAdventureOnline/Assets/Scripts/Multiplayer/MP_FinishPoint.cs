using UnityEngine;
using Fusion;
using System.Collections.Generic;
using System.Linq;

public class MP_FinishPoint : NetworkBehaviour
{
    private Animator anim;

    public override void Spawned()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Object.HasStateAuthority) return;

        MP_Player player = collision.GetComponent<MP_Player>();

        if (player != null && player.Object.HasInputAuthority)
        {
            float finishTime = (float)Runner.SimulationTime;
            MP_GameManager.instance.RPC_SaveFinishTime(player.Object.InputAuthority, finishTime);

            RPC_TriggerFinishAnimation();
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
}