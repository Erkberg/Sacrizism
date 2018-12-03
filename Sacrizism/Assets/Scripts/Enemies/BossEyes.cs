using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyes : MonoBehaviour
{
    public Transform leftEye;
    public Transform rightEye;

    public Transform bulletPrefab;

    public void TakeDamage(int amount)
    {
        GameManager.instance.OnBossTakeDamage(amount);
    }
}
