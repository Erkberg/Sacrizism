using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private const float destroyDelay = 8f;

    public float moveSpeed = 8f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    public void SetDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.ObstacleTag))
        {
            Destroy();
        }

        if (collision.CompareTag(Tags.EnemyTag))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
