using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    void Awake()
    {
        if(!player)
        {
            player = GameObject.FindGameObjectWithTag(Tags.PlayerTag).transform;
        }
    }

    void Update()
    {
        transform.position = player.position;
    }
}
