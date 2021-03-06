﻿using System.Collections;
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
    public ChangeFacing changeFacing;
    public Animator animator;
    public Animator shadowAnimator;

    protected Transform player;

    public bool hasPowerUp = false;
    protected bool isAngered = false;
    protected bool isActive = false;
    protected bool isMoving = false;

    private void Awake()
    {
        player = GameManager.instance.player;
        float random = GameManager.instance.GetSmallRandomizer();
        moveSpeed *= random;
        animator.speed *= random;
        shadowAnimator.speed *= random;

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

            CheckMovingAnimation();
        }
    }

    protected virtual void OnFixedUpdate() { }

    public void SetAngered()
    {
        if(!isAngered)
        {
            isAngered = true;
            OnSetAngered();

            if (enemyGroup)
            {
                enemyGroup.SetAngered();
            }
        }        
    }

    public void Unanger()
    {
        isAngered = false;
        rb2D.velocity = Vector2.zero;
        CheckMovingAnimation();
    }

    protected virtual void OnSetAngered() { }

    public void SetLevel(int level)
    {
        this.level = level;

        float bColor = 1f - (float) level / maxLevel;
        spriteRenderer.color = new Color(1f, bColor, 1f, 1f);

        OnSetLevel();
    }

    protected virtual void OnSetLevel() { }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.BulletAreaTag))
        {
            SetActive(true);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.BulletAreaTag))
        {
            SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;

        OnSetActive(active);
    }

    protected virtual void OnSetActive(bool active) { }

    protected void CheckMovingAnimation()
    {
        if(rb2D.velocity.sqrMagnitude < 1f)
        {
            rb2D.velocity = Vector2.zero;
        }

        if (rb2D.velocity == Vector2.zero)
        {
            SetMoving(false);
        }
        else
        {
            SetMoving(true);
        }
    }

    protected void SetMoving(bool moving)
    {
        if (moving == isMoving)
        {
            return;
        }

        isMoving = moving;

        animator.SetBool(AnimationBools.MovingBool, moving);
        shadowAnimator.SetBool(AnimationBools.MovingBool, moving);
    }

    public void RemoveRigidbodyConstraints()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetDancing()
    {
        animator.SetTrigger("Dance");
        shadowAnimator.SetTrigger("Dance");
    }
}

public enum EnemyType
{
    Runner,
    Shooter,
    Healer,
    Boss
}