using Fusion;
using UnityEngine;
using System.Collections;

public class MP_Player : NetworkBehaviour
{
    [SerializeField] private GameObject fruitDrop;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;

    [Networked] private NetworkBool canBeController { get; set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float doubleJumpForce = 13f;
    private float defaultGravityScale;
    [Networked] private NetworkBool canDoubleJump { get; set; }

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float wallCheckDistance = 0.8f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius = 0.4f;
    [SerializeField] private LayerMask whatIsEnemy;
    [Networked] private NetworkBool isGrounded { get; set; }
    [Networked] private NetworkBool isWallDetected { get; set; }
    [Networked] private NetworkBool facingRight { get; set; } = true;
    [Networked] private int facingDir { get; set; } = 1;

    [Header("Player Visuals")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private ParticleSystem dustFx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public override void Spawned()
    {
        defaultGravityScale = rb.gravityScale;
        canBeController = true;
        facingRight = true;
        canDoubleJump = true;

        if (Object.HasStateAuthority)
        {
            RespawnFinished(false);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        HandleCollision();

        if (canBeController == false) return;

        if (GetInput(out NetworkInputData data))
        {
            HandleMovement(data);
            HandleJump(data);
        }

        UpdateAnimation();
    }

    private void HandleCollision()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = groundHit.collider != null;

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        isWallDetected = wallHit.collider != null;

        if (isGrounded)
            canDoubleJump = true;
    }

    private void HandleMovement(NetworkInputData data)
    {
        float moveInput = data.direction.x;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0 && facingDir != 1)
            Flip();
        else if (moveInput < 0 && facingDir != -1)
            Flip();
    }

    private void UpdateAnimation()
    {
        if (Object.HasStateAuthority)
        {
            anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    private void HandleJump(NetworkInputData data)
    {
        if (data.jump)
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump)
            {
                DoubleJump();
            }
            else if (isWallDetected)
            {
                WallJump();
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (Object.HasStateAuthority)
            dustFx.Play();
    }

    private void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        canDoubleJump = false;
    }

    private void WallJump()
    {
        rb.linearVelocity = new Vector2(-facingDir * moveSpeed, jumpForce);
        Flip();
    }

    private void Flip()
    {
        facingDir *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDir;
        transform.localScale = scale;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Die()
    {
        if (Object.HasStateAuthority)
        {
            if (deathVFX != null)
                Instantiate(deathVFX, transform.position, Quaternion.identity);

            Runner.Despawn(Object);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDir, transform.position.y));

        if (enemyCheck != null)
            Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_RespawnFinished(bool finished)
    {
        if (finished)
        {
            rb.gravityScale = defaultGravityScale;
            canBeController = true;
            cd.enabled = true;
        }
        else
        {
            rb.gravityScale = 0;
            canBeController = false;
            cd.enabled = false;
        }
    }

    public void RespawnFinished(bool finished)
    {
        if (Object == null || !Object.IsValid) return;

        if (Object.HasStateAuthority)
        {
            RPC_RespawnFinished(finished);
        }
    }

    public void PlayDustEffect()
    {
        if (Object.HasStateAuthority && dustFx != null)
        {
            dustFx.Play();
        }
    }
}