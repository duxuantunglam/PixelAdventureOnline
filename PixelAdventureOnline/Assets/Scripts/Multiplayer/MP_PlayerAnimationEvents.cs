using UnityEngine;

public class MP_PlayerAnimationEvents : MonoBehaviour
{
    private MP_Player player;

    private void Awake()
    {
        player = GetComponentInParent<MP_Player>();

        if (player == null)
        {
            Transform root = transform.root;
            if (root != null)
            {
                player = root.GetComponent<MP_Player>();
            }
        }

        if (player == null)
        {
            Debug.LogError("MP_PlayerAnimationEvents: There is no MP_Player component!");
        }
    }

    public void FinishRespawn()
    {
        if (player != null)
        {
            player.RespawnFinished(true);
        }
        else
        {
            Debug.LogError("MP_PlayerAnimationEvents: player is null in FinishRespawn!");
        }
    }

    public void PlayDustFX()
    {
        if (player != null)
        {
            player.PlayDustEffect();
        }
        else
        {
            Debug.LogError("MP_PlayerAnimationEvents: player is null in PlayDustFX!");
        }
    }
}