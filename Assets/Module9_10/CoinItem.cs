using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SFXPlayer.instance.Playhitcoin();
            this.gameObject.SetActive(false);
        }
    }
}
