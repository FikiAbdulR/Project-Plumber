using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Goomba goomba = other.GetComponent<Goomba>();
            if (goomba != null)
            {
                goomba.EnemyActive = true;
            }
        }
    }
}
