using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    public Transform rightSensor;
    private Rigidbody2D rb;
    private Animator animator;

    public bool EnemyActive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!animator.GetBool("isDead") && EnemyActive)
        {
            Move();
            CheckObstacles();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // Move left by setting x-component to negative
    }

    void CheckObstacles()
    {
        // Check for obstacles on the right side
        RaycastHit2D rightHit = Physics2D.Raycast(rightSensor.position, Vector2.right, 0.1f, obstacleLayer);

        // If an obstacle is detected on the right side, change direction
        if (rightHit.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Change the Goomba's direction
        moveSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player") && MarioController.instance.isInvincible == false)
        {
            // Handle player collision, for example, call SetIsDead(true)
            animator.SetBool("isDead", true);
            SFXPlayer.instance.Playhitgoomba();
            rb.velocity = Vector2.zero; // Stop movement when dead
        }
    }

    public void InActiveObject()
    {
        this.gameObject.SetActive(false);
    }
}
