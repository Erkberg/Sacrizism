using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Character character;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag(Tags.EnemyTag))
        {
            character.TakeDamage(1);
            ShakeScreenOnDamage();
            transform.position += (transform.position - collision.collider.transform.position).normalized;
        }
    }

    public void ShakeScreenOnDamage()
    {
        GameManager.instance.cameraMovement.Shake(4f, 4f);
    }
}
