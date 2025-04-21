using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = .5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SP_Player player = collision.gameObject.GetComponent<SP_Player>();

        if (player != null)
        {
            player.Push(transform.up * pushPower, duration);
            anim.SetTrigger("activate");
        }
    }
}