using UnityEngine;

public class LevelCameraTrigger : MonoBehaviour
{
    private LevelCamera levelCamera;

    private void Awake()
    {
        levelCamera = GetComponentInParent<LevelCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SP_Player player = collision.gameObject.GetComponent<SP_Player>();

        if (player != null)
        {
            levelCamera.EnableCamera(true);
            levelCamera.SetNewTarget(player.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SP_Player player = collision.gameObject.GetComponent<SP_Player>();

        if (player != null)
        {
            levelCamera.EnableCamera(false);
        }
    }
}