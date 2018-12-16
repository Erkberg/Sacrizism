using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public Transform healParticlePrefab;
    public Transform deathParticlePrefab;
    public Transform bossHurtParticlePrefab;

    public Transform bloodHolder;
    public Transform[] bloodPrefabs;

    public Transform fireworksParticlePrefab;

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
            Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Length)], position, rotation, bloodHolder);
        }
    }

    public IEnumerator FireworksSequence()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while(true)
        {
            Instantiate(fireworksParticlePrefab, GameManager.instance.player.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-6f, 6f), 0f), Quaternion.identity);
            yield return delay;
        }
    }

    public void ClearBlood()
    {
        foreach(Transform blood in bloodHolder)
        {
            Destroy(blood.gameObject);
        }
    }
}
