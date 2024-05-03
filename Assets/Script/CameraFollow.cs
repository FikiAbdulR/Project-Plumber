using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's Transform
    public Vector2 minBounds; // Minimum camera position
    public Vector2 maxBounds; // Maximum camera position

    public float smoothSpeed = 0.125f; // Adjust the smoothing speed

    void LateUpdate()
    {
        if (target != null)
        {
            float clampedX = Mathf.Clamp(target.position.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(target.position.y, minBounds.y, maxBounds.y);

            Vector3 desiredPosition = new Vector3(clampedX, clampedY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    void OnDrawGizmos()
    {
        // Draw Gizmos to visualize camera bounds
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0f), new Vector3(maxBounds.x, minBounds.y, 0f));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0f), new Vector3(maxBounds.x, maxBounds.y, 0f));
        Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0f), new Vector3(minBounds.x, maxBounds.y, 0f));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0f), new Vector3(minBounds.x, minBounds.y, 0f));
    }
}
