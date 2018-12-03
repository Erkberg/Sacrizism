using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSounds;

    public AudioClip uiElementAppearSound;
    public AudioClip powerUpSound;

    public void PlayMusic()
    {
        audioSourceMusic.Play();
    }

    public void PlayUIElementAppearSound()
    {
        audioSourceSounds.PlayOneShot(uiElementAppearSound);
    }

    public void PlayPowerUpSound()
    {
        audioSourceSounds.PlayOneShot(powerUpSound);
    }
}
