using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public Transform healParticlePrefab;
    public Transform deathParticlePrefab;

    public void SpawnHealParticle(Transform atTransform)
    {
        Instantiate(healParticlePrefab, atTransform.position, Quaternion.identity, atTransform);
    }

    public void SpawnDeathParticle(Transform atTransform)
    {
        Instantiate(deathParticlePrefab, atTransform.position, Quaternion.identity);
    }

    public void SpawnDeathParticle(Vector3 atPosition)
    {
        Instantiate(deathParticlePrefab, atPosition, Quaternion.identity);
    }
}
