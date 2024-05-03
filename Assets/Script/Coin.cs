using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
}
