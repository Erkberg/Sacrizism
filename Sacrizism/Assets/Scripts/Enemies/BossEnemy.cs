using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public BossEyes bossEyes;

    public IEnumerator Die()
    {
        bossEyes.OnDeath();
        yield return new WaitForSeconds(0.2f);
        GameManager.instance.cameraMovement.Shake(4f, 60f);
        for(int i = 0; i < 50; i++)
        {
            GameManager.instance.particlesManager.SpawnDeathParticle(transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
