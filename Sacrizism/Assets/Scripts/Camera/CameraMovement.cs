using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private readonly Vector2 baseIntensity = new Vector2(0.1f, 0.1f);
    private const float baseDuration = 0.1f;

    public bool isFollowing = true;
    
    public Transform mainCam;

    private float followSmoothTime = 0.33f;
    private Vector3 followRefVelocity;

    private float shakeSmoothTime = 0.05f;
    private Vector3 shakeRefVelocity;

    private Vector3 camToPlayerOffset = new Vector3(0f, 0f, -10f);

    private Transform player;

    private bool movingTowardsTarget = false;
    private Vector3 currentTargetPosition;
    private float moveTowardsTargetSmoothTime = 0.5f;
    private Vector3 moveTowardsTargetRefVelocity;

    private float initialZoom;

    private void Awake()
    {
        player = GameManager.instance.player;
        initialZoom = mainCam.GetComponent<Camera>().orthographicSize;
    }

    private void FixedUpdate()
    {
        if(player && isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.position + camToPlayerOffset, ref followRefVelocity, followSmoothTime);
        }
        else if(movingTowardsTarget)
        {
            transform.position = Vector3.SmoothDamp(transform.position, currentTargetPosition + camToPlayerOffset, ref moveTowardsTargetRefVelocity, moveTowardsTargetSmoothTime);

            if(Vector3.Distance(currentTargetPosition, transform.position) < 0.01f)
            {
                movingTowardsTarget = false;
            }
        }            
    }

    public void MoveToTargetPosition(Vector3 position)
    {
        movingTowardsTarget = true;
        currentTargetPosition = position;
    }

    public void Shake(float intensityMultiplier = 1f, float durationMultiplier = 1f)
    {
        StopAllCoroutines();

        StartCoroutine(ShakeSequence(baseIntensity * intensityMultiplier, baseDuration * durationMultiplier));
    }

    public void Shake(Vector2 intensity, float intensityMultiplier = 1f, float durationMultiplier = 1f)
    {
        StopAllCoroutines();

        StartCoroutine(ShakeSequence(intensity * intensityMultiplier, baseDuration * durationMultiplier));
    }

    private IEnumerator ShakeSequence(Vector2 intensity, float duration)
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

            currentTarget.x = Random.Range(-intensity.x, intensity.x);
            currentTarget.y = Random.Range(-intensity.y, intensity.y);

            mainCam.transform.localPosition = Vector3.SmoothDamp(mainCam.transform.localPosition, currentTarget, ref shakeRefVelocity, shakeSmoothTime);

            yield return null;
        }

        mainCam.transform.localPosition = Vector3.zero;
    }

    public IEnumerator ShortZoomOut()
    {
        Camera mainCamCamera = mainCam.GetComponent<Camera>();
        float offset = 0.5f;
        float remainingPercentage = 1f;

        while (mainCamCamera.orthographicSize < initialZoom + offset * 0.9f)
        {
            remainingPercentage = (initialZoom + offset - mainCamCamera.orthographicSize) / offset;
            mainCamCamera.orthographicSize += Time.deltaTime * 4f * remainingPercentage;
            yield return null;
        }

        while(mainCamCamera.orthographicSize > initialZoom)
        {
            mainCamCamera.orthographicSize -= Time.deltaTime;
            yield return null;
        }

        mainCamCamera.orthographicSize = initialZoom;
    }
}
