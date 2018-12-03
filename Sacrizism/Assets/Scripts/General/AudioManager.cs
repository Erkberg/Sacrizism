using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSounds;
    public AudioSource audioSourceAtmo;

    public AudioClip powerUpSound;
    public AudioClip bossArrivalSound;

    public AudioClip shootSound;
    public AudioClip wamsSound;
    public AudioClip wamsShortSound;

    public AudioClip[] playerHurtSounds;
    public AudioClip[] enemyHurtSounds;
    public AudioClip[] kachuHurtSounds;

    public void PlayMusic()
    {
        audioSourceMusic.Play();
    }

    public void PlayAtmo()
    {
        audioSourceAtmo.Play();
    }

    public void PlayUIElementAppearSound()
    {
        audioSourceSounds.PlayOneShot(wamsShortSound);
    }

    public void PlayWamsSound()
    {
        audioSourceSounds.PlayOneShot(wamsSound);
    }

    public void PlayBossArrivalSound()
    {
        audioSourceSounds.PlayOneShot(bossArrivalSound);
    }

    public void PlayShootSound()
    {
        audioSourceSounds.PlayOneShot(shootSound);
    }

    public void PlayPowerUpSound()
    {
        audioSourceSounds.PlayOneShot(powerUpSound);
    }

    public void PlayKachuHurtSound()
    {
        if (kachuHurtSounds != null && kachuHurtSounds.Length > 0)
        {
            audioSourceSounds.PlayOneShot(kachuHurtSounds[Random.Range(0, kachuHurtSounds.Length)]);
        }
    }

    public void PlayPlayerHurtSound()
    {
        if(playerHurtSounds != null && playerHurtSounds.Length > 0)
        {
            audioSourceSounds.PlayOneShot(playerHurtSounds[Random.Range(0, playerHurtSounds.Length)]);
        }
    }

    public void PlayEnemyHurtSound()
    {
        if (enemyHurtSounds != null && enemyHurtSounds.Length > 0)
        {
            audioSourceSounds.PlayOneShot(enemyHurtSounds[Random.Range(0, enemyHurtSounds.Length)]);
        }
    }
}
