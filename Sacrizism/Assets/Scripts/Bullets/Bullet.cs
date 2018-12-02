using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float destroyDelay = 8f;

    public float moveSpeed = 8f;
    public int damage = 1;

    protected void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    public void SetDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
    }

    protected void Destroy()
    {
        Destroy(gameObject);
    }
}
