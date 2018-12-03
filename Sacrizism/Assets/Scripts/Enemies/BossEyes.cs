using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyes : MonoBehaviour
{
    public Transform leftEye;
    public Transform rightEye;

    public Transform bulletsHolder;
    public Transform bulletPrefab;

    private int previousSequenceID = 0;
    private readonly Vector3 standardDirection = new Vector3(0f, -1f, 0f);

    private void Awake()
    {
        bulletsHolder = GameObject.FindGameObjectWithTag(Tags.BulletsHolderTag).transform;
    }

    public void TakeDamage(int amount)
    {
        GameManager.instance.OnBossTakeDamage(amount);
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        Destroy(bulletsHolder.gameObject);
    }

    public void StartLoop()
    {
        StartCoroutine(ShootingLoop());
    }

    private IEnumerator ShootingLoop()
    {
        while(true)
        {
            yield return PlayRandomShootSequence();
        }
    }

    private IEnumerator PlayRandomShootSequence()
    {
        float step = 1f / 3;

        float random = Random.Range(0f, 1f);

        if(random <= step)
        {
            yield return ShotSequenceBig();
        }
        else if(random <= step * 2)
        {
            yield return ShotSequenceSmall();
        }
        else if (random <= step * 3)
        {
            yield return ShotSequenceRandom();
        }
    }

    private IEnumerator ShotSequenceBig()
    {
        float size = 1f;
        float speed = 4f;
        float waitTime = 1.1f;

        // waves
        for (int i = 0; i < 8; i++)
        {
            // bullets
            for(int j = -5; j < 5; j++)
            {
                float angle = j * 22f + Random.Range(-8f, 8f);

                SpawnBulletAtAngle(angle, size, speed, leftEye);
                SpawnBulletAtAngle(angle, size, speed, rightEye);

                GameManager.instance.cameraMovement.Shake(2f);
            }

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator ShotSequenceSmall()
    {
        float size = 0.2f;
        float speed = 7.5f;
        float waitTime = 0.1f;

        // waves
        for (int i = -16; i < 16; i++)
        {
            float angle = i * 10f;

            SpawnBulletAtAngle(angle, size, speed, leftEye);
            SpawnBulletAtAngle(-angle, size, speed, rightEye);

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ShotSequenceRandom()
    {
        float size = 0.33f;
        float speed = 8f;
        float waitTime = 0.025f;

        // waves
        for (int i = 0; i < 8; i++)
        {
            // bullets
            for (int j = -4; j < 4; j++)
            {
                float angle = Random.Range(-120f, 120f);
                SpawnBulletAtAngle(angle, size, speed, leftEye);
                yield return new WaitForSeconds(waitTime);
                angle = Random.Range(-120f, 120f);
                SpawnBulletAtAngle(angle, size, speed, rightEye);
                yield return new WaitForSeconds(waitTime);
            }
        }

        yield return new WaitForSeconds(1f);
    }

    private void SpawnBulletAtAngle(float angle, float size, float speed, Transform origin, int damage = 1)
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, origin.position, Quaternion.identity, bulletsHolder).GetComponent<EnemyBullet>();
        bullet.SetSize(size);
        bullet.SetMoveSpeed(speed);
        bullet.SetDamage(damage);
        bullet.SetDirection(Quaternion.Euler(0, 0, angle) * standardDirection);
    }
}
