using UnityEngine;
using UnityEngine.Windows;

public class MenuCharacter : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 destination;
    private Animator anim;

    private bool isMoving;
    private int facingDir = 1;
    private bool facingRight = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                isMoving = false;

                if (!facingRight)
                {
                    Flip();
                }
            }
        }
    }

    public void MoveTo(Transform newDestination)
    {
        destination = newDestination.position;
        destination.y = transform.position.y;

        isMoving = true;
        HandleFlip(destination.x);
    }

    public void TeleportTo(Transform newDestination)
    {
        destination = newDestination.position;
        destination.y = transform.position.y;

        transform.position = destination;

        isMoving = false;
        HandleFlip(destination.x);
    }

    private void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * (-1);
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}