using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SP_Player player = collision.GetComponent<SP_Player>();

        if (player != null)
        {
            anim.SetTrigger("activate");
        }
    }
}
