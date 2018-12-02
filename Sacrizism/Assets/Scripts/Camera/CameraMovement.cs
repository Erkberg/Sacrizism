using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float baseIntensity = 0.1f;
    private const float baseDuration = 0.1f;
    
    public Transform mainCam;

    private float followSmoothTime = 0.2f;
    private Vector3 followRefVelocity;

    private float shakeSmoothTime = 0.05f;
    private Vector3 shakeRefVelocity;

    private Vector3 camToPlayerOffset = new Vector3(0f, 0f, -10f);

    private Transform player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void FixedUpdate()
    {
        if(player)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.position + camToPlayerOffset, ref followRefVelocity, followSmoothTime);
        }
    }

    public void Shake(float intensityMultiplier = 1f, float durationMultiplier = 1f)
    {
        StopAllCoroutines();

        StartCoroutine(ShakeSequence(baseIntensity * intensityMultiplier, baseDuration * durationMultiplier));
    }

    private IEnumerator ShakeSequence(float intensity, float duration)
    {
        float durationPassed = 0f;
        Vector3 currentTarget = Vector3.zero;

        while(true)
        {
            durationPassed += Time.deltaTime;

            if(durationPassed >= duration)
            {
                break;
            }

            currentTarget.x = Random.Range(-intensity, intensity);
            currentTarget.y = Random.Range(-intensity, intensity);

            mainCam.transform.localPosition = Vector3.SmoothDamp(mainCam.transform.localPosition, currentTarget, ref shakeRefVelocity, shakeSmoothTime);

            yield return null;
        }

        mainCam.transform.localPosition = Vector3.zero;
    }
}
