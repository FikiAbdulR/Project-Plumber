using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer instance;

    public AudioSource audioSource;
    public AudioSource music;

    public AudioClip PlayerJump;
    public AudioClip SuperJump;
    public AudioClip Playerpowerup;
    public AudioClip GameOver;

    public AudioClip hitbrick;
    public AudioClip destroybrick;

    public AudioClip hitcoin;
    public AudioClip hitMushroom;

    public AudioClip hitgoomba;

    public AudioClip goTunnel;
    public AudioClip pole;
    public AudioClip ClearLevel;

    public bool musicground = true;
    public AudioClip[] MusicClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        music.clip = MusicClip[0];
        music.Play();
    }

    public void Playhitbrick()
    {
        audioSource.PlayOneShot(hitbrick);
    }

    public void Playdestroybrick()
    {
        audioSource.PlayOneShot(destroybrick);
    }

    public void Playhitcoin()
    {
        audioSource.PlayOneShot(hitcoin);
    }

    public void PlayhitMushroom()
    {
        audioSource.PlayOneShot(hitMushroom);
    }

    public void Playhitgoomba()
    {
        audioSource.PlayOneShot(hitgoomba);
    }

    public void PlayPlayerjump()
    {
        audioSource.PlayOneShot(PlayerJump);
    }

    public void PlaySuperjump()
    {
        audioSource.PlayOneShot(SuperJump);
    }

    public void PlayPlayerpowerup()
    {
        audioSource.PlayOneShot(Playerpowerup);
    }

    public void PlaygoTunnel()
    {
        audioSource.PlayOneShot(goTunnel);
    }

    public void LevelClear()
    {
        StartCoroutine(PlaySoundsWithDelay());
    }

    private IEnumerator PlaySoundsWithDelay()
    {
        music.enabled = false;
        audioSource.PlayOneShot(pole);

        // Wait for 1 second before playing ClearLevel
        yield return new WaitForSeconds(1.1f);

        audioSource.PlayOneShot(ClearLevel);
    }

    public void PlayGameover()
    {
        music.enabled = false;
        audioSource.PlayOneShot(GameOver);
    }

    public void ChangeMusic(bool state)
    {
        musicground = state;

        // Toggle between audio sources
        if (musicground == true)
        {
            music.clip = MusicClip[0];
            music.Play();
        }

        else if(musicground == false)
        {
            music.clip = MusicClip[1];
            music.Play();
        }
    }
}