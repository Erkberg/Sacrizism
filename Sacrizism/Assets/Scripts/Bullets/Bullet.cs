using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 8f;
    public int damage = 1;
    public int pierce = 0;

    private IEnumerator Start()
    {
        if(GameManager.instance.gameState == GameState.Level)
        {
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().sortingOrder = -9;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
    }

    public void AddDamage(int bonusDamage)
    {
        damage += bonusDamage;
    }

    public void AddMoveSpeed(float bonusMoveSpeed)
    {
        moveSpeed += bonusMoveSpeed;
    }

    public void AddSize(float bonusSize)
    {
        float size = transform.localScale.x + bonusSize;
        transform.localScale = new Vector3(size, size, 1f);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetSize(float newSize)
    {
        transform.localScale = new Vector3(newSize, newSize, 1f);
    }

    public void AddPierce(int bonusPierce)
    {
        pierce += bonusPierce;
    }

    protected void Destroy()
    {
        Destroy(gameObject);
    }

    protected bool DoesPierce()
    {
        if(pierce > 0)
        {
            pierce--;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.BulletAreaTag))
        {
            Destroy();
        }
    }
}
