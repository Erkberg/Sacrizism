using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemy : Enemy
{
    private const float runningAwayProbability = 0.33f;

    private bool runningAway = false;

    protected override void OnAwake()
    {
        if(Random.Range(0f, 1f) < runningAwayProbability)
        {
            runningAway = true;
        }
    }

    protected override void OnFixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(runningAway)
        {
            rb2D.velocity = (transform.position - player.transform.position).normalized * moveSpeed;
        }
        else
        {
            rb2D.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
        }

        CheckFacing();
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
        if(level == 1)
        {
            moveSpeed *= 1.5f;
            character.SetMaxHP(6);
        }

        if(level == 2)
        {
            moveSpeed *= 2f;
            character.SetMaxHP(9);
        }
    }
}
