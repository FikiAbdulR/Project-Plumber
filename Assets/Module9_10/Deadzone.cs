using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collision is with the player
        if (other.CompareTag("Player"))
        {
            MarioController.instance.Die();
        }
    }
}
