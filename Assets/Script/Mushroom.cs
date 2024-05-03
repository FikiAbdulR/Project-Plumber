using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    public Transform rightSensor;
    public LayerMask obstacleLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        CheckObstacles();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && MarioController.instance.isPoweredUp == false)
        {
            MarioController.instance.CollectPowerUp();
            this.gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Player") && MarioController.instance.isPoweredUp == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
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
}
