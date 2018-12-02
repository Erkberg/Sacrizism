using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int maxLevel = 2;

    public EnemyType enemyType;
    public EnemyGroup enemyGroup;
    public int level = 0;
    public float moveSpeed = 2f;

    public Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public Character character;

    protected Transform player;

    private bool isAngered = false;
    private bool isActive = false;

    private void Awake()
    {
        player = GameManager.instance.player;

        OnAwake();
    }

    protected virtual void OnAwake() { }

    // Update is called once per frame
    private void Update ()
    {
        if(isActive && isAngered)
        {
            OnUpdate();
        }        
	}

    protected virtual void OnUpdate() { }

    private void FixedUpdate()
    {
        if (isActive && isAngered)
        {
            OnFixedUpdate();
        }
    }

    protected virtual void OnFixedUpdate() { }

    public void SetAngered()
    {
        if(!isAngered)
        {
            isAngered = true;

            if (enemyGroup)
            {
                enemyGroup.SetAngered();
            }
        }        
    }

    public void SetLevel(int level)
    {
        this.level = level;

        float gbColor = 1f - (float) level / maxLevel;
        spriteRenderer.color = new Color(1f, gbColor, gbColor, 1f);

        OnSetLevel();
    }

    protected virtual void OnSetLevel() { }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.BulletAreaTag))
        {
            isActive = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.BulletAreaTag))
        {
            isActive = false;
        }
    }
}

public enum EnemyType
{
    Runner,
    Shooter,
    Healer,
    Boss
}