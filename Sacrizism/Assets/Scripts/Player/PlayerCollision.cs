using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Character character;
    public PlayerPowerUps playerPowerUps;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.collider.CompareTag(Tags.EnemyTag) && !GameManager.instance.enemyManager.enemiesPeaceful) || collision.collider.CompareTag(Tags.BossTag))
        {
            character.TakeDamage(1);
            ShakeScreenOnDamage();
            transform.position += (transform.position - collision.collider.transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PowerUpTag))
        {
            playerPowerUps.OnPowerUpPickedUp();
            Destroy(collision.gameObject);
        }
    }

    public void ShakeScreenOnDamage()
    {
        GameManager.instance.cameraMovement.Shake(4f, 4f);
    }
}
