using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MarioController : MonoBehaviour
{
    public static MarioController instance;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float crouchSpeed = 2f; // Adjust the crouch movement speed as needed
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isCrouching = false; // New variable to track crouch state

    public Collider2D normalCollider;
    public Collider2D poweredUpCollider;

    public bool isPoweredUp = false;

    public bool isInvincible = false;
    private float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;

    bool isDead = false;
    bool animating;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Crouch(); // Call the new Crouch function
        FlipSprite();
        UpdateAnimator();

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            // Check if the invincibility period has ended
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    void Move()
    {
        if(!isDead)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (!isCrouching) // Only apply normal movement if not crouching
            {
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
                animator.SetBool("IsWalking", Mathf.Abs(horizontalInput) > 0.1f);
            }
            else
            {
                // Player shouldn't move while crouching
                rb.velocity = new Vector2(0f, rb.velocity.y);
                animator.SetBool("IsWalking", false);
            }
        }
    }


    void Jump()
    {
        if(!isDead)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

            if (isGrounded && Input.GetButtonDown("Jump") && isCrouching == false)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("IsJumping");

                if(isPoweredUp == true)
                {
                    SFXPlayer.instance.PlaySuperjump();
                }
                else
                {
                    SFXPlayer.instance.PlayPlayerjump();
                }
            }
        }
    }

    void Crouch()
    {
        if(!isDead)
        {
            if (isPoweredUp == true)
            {
                // Toggle crouch state
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    isCrouching = true;
                    animator.SetBool("IsCrouching", true);

                    // Disable the normal collider and enable the crouching collider
                    if (normalCollider != null && poweredUpCollider != null)
                    {
                        normalCollider.enabled = true;
                        poweredUpCollider.enabled = false; // You may need to adjust this based on your requirements
                    }
                }
                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    isCrouching = false;
                    animator.SetBool("IsCrouching", false);

                    // Enable the normal collider and disable the crouching collider
                    if (normalCollider != null && poweredUpCollider != null)
                    {
                        normalCollider.enabled = false;
                        poweredUpCollider.enabled = isPoweredUp; // You may need to adjust this based on your requirements
                    }
                }
            }
        }
    }

    void FlipSprite()
    {
        if(!isDead)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0 && !isFacingRight || horizontalInput < 0 && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                GetComponent<SpriteRenderer>().flipX = !isFacingRight;
            }
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void CollectPowerUp()
    {
        isPoweredUp = true;
        animator.SetBool("GetBuff", isPoweredUp);
        SFXPlayer.instance.PlayPlayerpowerup();

        // Change collider
        if (normalCollider != null && poweredUpCollider != null)
        {
            normalCollider.enabled = !isCrouching; // Enable only if not crouching
            poweredUpCollider.enabled = isPoweredUp && !isCrouching; // Enable only if powered up and not crouching
        }

        // Add logic here for any additional effects or actions when collecting the power-up
    }

    public void DamageCond()
    {
        if (!isInvincible)
        {
            if (isPoweredUp)
            {
                // Mario is on power-up state, turn into normal state
                TakeDamage();
            }
            else if (!isPoweredUp)
            {
                // Mario is on normal state, he gets hit and dies
                Die();
            }
        }
    }

    void TakeDamage()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        isPoweredUp = false;
        animator.SetBool("GetBuff", isPoweredUp);

        // Change collider back to normal
        if (normalCollider != null && poweredUpCollider != null)
        {
            normalCollider.enabled = !isCrouching; // Enable only if not crouching
            poweredUpCollider.enabled = isPoweredUp && !isCrouching; // Enable only if powered up and not crouching
        }

        // Add logic here for any additional effects or actions when taking damage
        StartCoroutine(FlickerEffect());
    }

    private IEnumerator FlickerEffect()
    {
        float flickerDuration = 0.2f;
        float flickerInterval = 0.1f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        while (invincibilityTimer > 0f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerInterval);
        }

        spriteRenderer.enabled = true;
    }

    public void Die()
    {
        // Play death animation or any other death-related logic
        isDead = true;
        normalCollider.enabled = false;
        poweredUpCollider.enabled = false;

        animator.SetBool("IsDead", true);
        SFXPlayer.instance.PlayGameover();

        animating = true;

        rb.velocity = Vector2.zero; // Stop movement when dead

        StartCoroutine(AnimateAndRestart());
        // Delay the scene restart by calling the RestartScene method after a brief moment
    }

    private IEnumerator AnimateAndRestart()
    {
        yield return StartCoroutine(Animate());
        yield return new WaitForSeconds(4.5f);
        RestartScene();
    }

    private IEnumerator Animate()
    {
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        Debug.Log("Animating started");
        if (animating)
        {
            Debug.Log("Already animating, exiting coroutine");
            yield break; // If already animating, exit the coroutine
        }

        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 1f;
        Vector3 animatedPosition1 = restingPosition + Vector3.up * 2f;

        yield return new WaitForSeconds(1f); // Delay at the top position
        yield return Move(animatedPosition, animatedPosition1); // Go down

        animating = false;
        Debug.Log("Animating completed");
    }

    private IEnumerator Move(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        float duration = 0.75f; // Adjust the duration as needed

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end; // Ensure the final position is exactly the target position
    }

    void RestartScene()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

}