using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    public PostProcessVolume volumeChromaticAberration;
    public PostProcessVolume volumeBloom;
    public PostProcessVolume volumeGrain;
    public PostProcessVolume volumeVignette;
    public PostProcessVolume volumeColorGrading;

    private void Awake()
    {
        volumeChromaticAberration.enabled = false;
        volumeBloom.enabled = false;
        volumeVignette.enabled = false;
        volumeGrain.enabled = false;
    }

    public void EnableVolume(PowerUpOnceType powerUp)
    {
        switch(powerUp)
        {
            case PowerUpOnceType.Psychedelic:
                volumeChromaticAberration.enabled = true;
                break;

            case PowerUpOnceType.InBloom:
                volumeBloom.enabled = true;
                break;

            case PowerUpOnceType.Film:
                volumeGrain.enabled = true;
                break;

            case PowerUpOnceType.Noir:
                volumeColorGrading.enabled = true;
                break;

            case PowerUpOnceType.TunnelVision:
                volumeVignette.enabled = true;
                break;
        }
    }
}
