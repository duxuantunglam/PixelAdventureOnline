using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SP_Player player = collision.gameObject.GetComponent<SP_Player>();

        if (player != null)
        {
            player.Damage();
            player.Die();
            PlayerManager.instance.RespawnPlayer();
        }

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
            enemy.Die();
    }
}