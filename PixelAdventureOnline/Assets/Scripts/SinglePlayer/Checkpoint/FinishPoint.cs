using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SP_Player player = collision.GetComponent<SP_Player>();

        if (player != null)
        {
            AudioManager.instance.PlaySFX(2);

            anim.SetTrigger("active");
            GameManager.instance.LevelFinished();
        }
    }
}