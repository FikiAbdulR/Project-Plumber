using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Castle : MonoBehaviour
{
    public int nextWorld = 1;
    public int nextStage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<MarioController>().enabled = false;

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(6.5f);

        LoadLevel(nextWorld, nextStage);
    }

    public void LoadLevel(int world, int stage)
    {
        SceneManager.LoadScene($"{world}-{stage}");
    }

}