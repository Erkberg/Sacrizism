using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.ObstacleTag))
        {      
            //if(!DoesPierce())
            //{
                Destroy();
            //}
        }

        if (collision.CompareTag(Tags.EnemyTag))
        {
            collision.GetComponent<Character>().TakeDamage(damage);

            if (!DoesPierce())
            {
                Destroy();
            }
        }

        if (collision.CompareTag(Tags.BossEyesTag))
        {
            collision.GetComponent<BossEyes>().TakeDamage(damage);
            GameManager.instance.particlesManager.SpawnBossHurtParticle(transform.position);

            Destroy();

            GameManager.instance.cameraMovement.Shake(2f, 1.5f);
        }
    }
}
