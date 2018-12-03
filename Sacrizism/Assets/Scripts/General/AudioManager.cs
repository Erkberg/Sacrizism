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

    public AudioClip[] hurtSounds;
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
        if (hurtSounds != null && hurtSounds.Length > 0)
        {
            audioSourceSounds.PlayOneShot(kachuHurtSounds[Random.Range(0, kachuHurtSounds.Length)]);
        }
    }

    public void PlayHurtSound()
    {
        if(hurtSounds != null && hurtSounds.Length > 0)
        {
            audioSourceSounds.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
        }
    }
}
