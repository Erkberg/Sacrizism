using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Transform mainCam;

    public float followSmoothTime = 0.2f;

    private Vector3 refVelocity;
    private Vector3 camToPlayerOffset = new Vector3(0f, 0f, -10f);

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + camToPlayerOffset, ref refVelocity, followSmoothTime);
    }
}
