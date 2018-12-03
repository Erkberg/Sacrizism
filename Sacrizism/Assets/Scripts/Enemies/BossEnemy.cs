using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public BossEyes bossEyes;

    public void Die()
    {
        Destroy(gameObject);
    }
}
