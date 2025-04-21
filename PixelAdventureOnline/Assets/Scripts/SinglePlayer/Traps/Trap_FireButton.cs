using UnityEngine;

public class Trap_FireButton : MonoBehaviour
{
    private Animator anim;
    private Trap_Fire trapFire;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        trapFire = GetComponentInParent<Trap_Fire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SP_Player player = collision.gameObject.GetComponent<SP_Player>();

        if (player != null)
        {
            anim.SetTrigger("activate");
            trapFire.SwitchOffFire();
        }

    }
}