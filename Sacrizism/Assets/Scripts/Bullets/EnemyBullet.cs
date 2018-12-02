using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.ObstacleTag))
        {
            Destroy();
        }

        if (collision.CompareTag(Tags.PlayerTag))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
            Destroy();
        }
    }
}
