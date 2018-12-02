using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.ObstacleTag))
        {
            Destroy();
        }

        if (collision.CompareTag(Tags.EnemyTag))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
            Destroy();
        }
    }
}
