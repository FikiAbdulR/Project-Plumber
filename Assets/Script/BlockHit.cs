using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1;
    private bool animating;

    public bool ispowerup;
    public bool iscoin;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 blockCenter = new Vector2(transform.position.x, transform.position.y);

            // Calculate the absolute difference along the X-axis
            float deltaX = Mathf.Abs(contactPoint.x - blockCenter.x);

            // Check if the collision point is above the center of the block and within a certain X-axis tolerance
            if (contactPoint.y < blockCenter.y && deltaX < 0.5f)
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        maxHits--;

        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }
        else if (maxHits <= 0 && MarioController.instance.isPoweredUp)
        {
            SFXPlayer.instance.Playdestroybrick();
            gameObject.SetActive(false);
        }
        else if(maxHits <= 0)
        {
            SFXPlayer.instance.Playhitbrick();
        }

        if (gameObject.activeSelf) // Check if the GameObject is active before starting the coroutine
        {
            if (ispowerup == true && iscoin == false)
            {
                SFXPlayer.instance.PlayhitMushroom();
            }
            else if (ispowerup == false && iscoin == true)
            {
                SFXPlayer.instance.Playhitcoin();
            }

            if (item != null)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }

            StartCoroutine(Animate());
        }


    }


    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

}