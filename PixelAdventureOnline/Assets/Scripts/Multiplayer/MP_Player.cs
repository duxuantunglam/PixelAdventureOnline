using Fusion;
using UnityEngine;

public class MP_Player : NetworkBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // if (inputData.jumpPressed && isGrounded)
        // {
        //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        // }

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            rb.linearVelocity = data.direction * moveSpeed;
        }
    }
}