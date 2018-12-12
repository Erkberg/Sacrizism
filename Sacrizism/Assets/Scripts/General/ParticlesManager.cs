using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public Transform healParticlePrefab;
    public Transform deathParticlePrefab;
    public Transform bossHurtParticlePrefab;

    public Transform[] bloodPrefabs;

    public void SpawnHealParticle(Transform atTransform)
    {
        Instantiate(healParticlePrefab, atTransform.position, Quaternion.identity, atTransform);
    }

    public void SpawnDeathParticle(Transform atTransform)
    {
        Instantiate(deathParticlePrefab, atTransform.position, Quaternion.identity);
        SpawnBloodAtPosition(atTransform.position);
    }

    public void SpawnDeathParticle(Vector3 atPosition)
    {
        Instantiate(deathParticlePrefab, atPosition, Quaternion.identity);
        SpawnBloodAtPosition(atPosition);
    }

    public void SpawnBossHurtParticle(Vector3 atPosition)
    {
        Instantiate(bossHurtParticlePrefab, atPosition, Quaternion.identity);
    }

    public void SpawnBloodAtPosition(Vector3 position)
    {
        if(bloodPrefabs != null && bloodPrefabs.Length > 0)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Length)], position, Quaternion.identity);
        }
    }
}
