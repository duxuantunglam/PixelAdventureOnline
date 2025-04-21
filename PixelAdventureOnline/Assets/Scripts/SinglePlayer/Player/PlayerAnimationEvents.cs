using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private SP_Player player;

    private void Awake()
    {
        player = GetComponentInParent<SP_Player>();
    }

    public void FinishRespawn() => player.RespawnFinished(true);
}