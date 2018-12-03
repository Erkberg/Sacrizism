using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : Enemy
{
    private const float maxDistanceToTarget = 3f;
    private const float minDistanceToTarget = 2f;

    private Transform currentTarget;
    private int healAmount = 1;

    public float reloadTime = 2f;
    private float reloadTimePassed = 0f;
    private bool isReloading = false;

    protected override void OnFixedUpdate()
    {
        Move();
    }

    protected override void OnUpdate()
    {
        if (isReloading)
        {
            Reload();
        }
        else
        {
            Heal();
        }
    }

    private void Heal()
    {
        isReloading = true;

        if(currentTarget == null)
        {
            currentTarget = enemyGroup.FindInjuredEnemy(this);
        }

        if(currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) < maxDistanceToTarget)
        {
            currentTarget.GetComponent<Enemy>().character.Heal(healAmount);
            if(!currentTarget.GetComponent<Enemy>().character.IsInjured())
            {
                currentTarget = enemyGroup.FindInjuredEnemy(this);
            }
        }
    }

    private void Reload()
    {
        reloadTimePassed += Time.deltaTime;

        if (reloadTimePassed >= reloadTime)
        {
            isReloading = false;
            reloadTimePassed = 0f;
        }
    }

    private void Move()
    {
        if(currentTarget)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) > minDistanceToTarget)
            {
                rb2D.velocity = (currentTarget.position - transform.position).normalized * moveSpeed;
            }
        }
    }

    private void CheckFacing()
    {
        if (rb2D.velocity.x < 0f)
        {
            changeFacing.SetFacing(Facing.Left);
        }
        else if (rb2D.velocity.x > 0f)
        {
            changeFacing.SetFacing(Facing.Right);
        }
    }

    protected override void OnSetLevel()
    {
        if (level == 1)
        {
            animator.speed *= 1.5f;
            shadowAnimator.speed *= 1.5f;
            moveSpeed *= 1.5f;
            character.SetMaxHP(10);
            reloadTime *= 0.9f;
            healAmount = 2;
        }

        if (level == 2)
        {
            moveSpeed *= 2f;
            animator.speed *= 2f;
            shadowAnimator.speed *= 2f;
            character.SetMaxHP(15);
            reloadTime *= 0.8f;
            healAmount = 3;
        }

        reloadTime *= GameManager.instance.GetSmallRandomizer();
    }
}
