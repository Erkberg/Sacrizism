using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 8f;
    public int damage = 1;

    public void SetDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
    }

    protected void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.BulletAreaTag))
        {
            Destroy();
        }
    }
}
