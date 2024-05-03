using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        FlipSprite();
        UpdateAnimator();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        animator.SetBool("IsWalking", Mathf.Abs(horizontalInput) > 0.1f);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("IsJumping");
        }
    }

    void FlipSprite()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0 && !isFacingRight || horizontalInput < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            GetComponent<SpriteRenderer>().flipX = !isFacingRight;
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsJumping", !isGrounded);
    }
}
