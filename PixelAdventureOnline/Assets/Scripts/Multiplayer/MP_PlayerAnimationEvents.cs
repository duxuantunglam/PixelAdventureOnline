using UnityEngine;

public class MP_PlayerAnimationEvents : MonoBehaviour
{
    private MP_Player player;

    private void Awake()
    {
        // Tìm MP_Player trong parent hoặc root object
        player = GetComponentInParent<MP_Player>();

        if (player == null)
        {
            // Nếu không tìm thấy trong parent, tìm trong root
            Transform root = transform.root;
            if (root != null)
            {
                player = root.GetComponent<MP_Player>();
            }
        }

        if (player == null)
        {
            Debug.LogError("MP_PlayerAnimationEvents: Không tìm thấy MP_Player component!");
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
            Debug.LogError("MP_PlayerAnimationEvents: player is null trong FinishRespawn!");
        }
    }

    // Thêm các animation events khác nếu cần
    public void PlayDustFX()
    {
        if (player != null)
        {
            player.PlayDustEffect();
        }
        else
        {
            Debug.LogError("MP_PlayerAnimationEvents: player is null trong PlayDustFX!");
        }
    }
}