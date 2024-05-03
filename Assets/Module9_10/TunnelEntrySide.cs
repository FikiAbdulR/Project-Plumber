using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelEntrySide : MonoBehaviour
{
    public Transform Entrance;
    public bool onTunnel = false;
    bool gotunnel = false;

    public Camera camera1;
    public Camera camera2;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the player enters the tunnel area
        if (other.CompareTag("Player"))
        {
            Teleport(other.transform.gameObject);
            SFXPlayer.instance.ChangeMusic(true);
            SFXPlayer.instance.PlaygoTunnel();
        }
    }

    public void Teleport(GameObject Mario)
    {
        if (Mario != null)
        {
            // Teleport the object to the targetTransform's position
            Mario.transform.position = Entrance.position;

            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        camera1.enabled = false;
        camera2.enabled = true;

        camera1.GetComponent<AudioListener>().enabled = false;
        camera2.GetComponent<AudioListener>().enabled = true;
    }
}
