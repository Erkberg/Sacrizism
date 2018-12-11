using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 8f;
    public int damage = 1;
    public int pierce = 0;
    public float wobbleFactor = 16f;

    private bool isWobbling = false;
    private Vector2 initialDirection;
    private Vector2 wobbleDirection;
    private Vector3 previousWobble = Vector3.zero;
    private float lifeTime = 0f;

    private IEnumerator Start()
    {
        if(GameManager.instance.gameState == GameState.Level)
        {
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().sortingOrder = -9;
        }
    }

    private void Update()
    {
        if(isWobbling)
        {
            Wobble();
        }
    }

    private void Wobble()
    {
        Vector3 currentWobble = wobbleDirection * lifeTime * moveSpeed * Mathf.Sin(lifeTime * wobbleFactor) * Time.deltaTime;
        transform.position = transform.position + currentWobble - previousWobble;
        previousWobble = currentWobble;
        lifeTime += Time.deltaTime;
    }

    public void SetWobbling(bool wobbling)
    {
        isWobbling = wobbling;
    }

    public void SetDirection(Vector2 direction)
    {
        initialDirection = direction * moveSpeed;
        wobbleDirection = Vector2.Perpendicular(initialDirection);
        GetComponent<Rigidbody2D>().velocity = initialDirection;
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
