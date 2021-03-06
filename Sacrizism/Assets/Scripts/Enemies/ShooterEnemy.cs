﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    private const float minDistanceToPlayer = 4f;

    public Transform enemyBulletPrefab;
    public Transform bulletsHolder;
    public Transform muzzle;

    public float reloadTime = 1f;
    private float reloadTimePassed = 0f;
    private bool isReloading = false;
    private float bulletSpeedBonus = 0f;

    protected override void OnAwake()
    {
        GameObject bulletsHolderObject = GameObject.FindGameObjectWithTag(Tags.BulletsHolderTag);

        if(bulletsHolderObject)
        {
            bulletsHolder = bulletsHolderObject.transform;
        }
    }

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
            Shoot();
        }

        CheckFacing();
    }

    private void Move()
    {
        if(Vector3.Distance(player.transform.position, transform.position) > minDistanceToPlayer)
        {
            rb2D.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
        }  
    }

    private void CheckFacing()
    {
        if (player.transform.position.x < transform.position.x)
        {
            changeFacing.SetFacing(Facing.Left);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            changeFacing.SetFacing(Facing.Right);
        }
    }

    protected override void OnSetLevel()
    {
        if (level == 1)
        {
            character.SetMaxHP(8);
            bulletSpeedBonus = 2f;
            reloadTime *= 0.75f;
        }

        if (level == 2)
        {
            character.SetMaxHP(12);
            bulletSpeedBonus = 4f;
            reloadTime *= 0.5f;
        }

        reloadTime *= GameManager.instance.GetSmallRandomizer();
    }

    private void Shoot()
    {
        isReloading = true;

        EnemyBullet bullet = Instantiate(enemyBulletPrefab, muzzle.position, Quaternion.identity, bulletsHolder).GetComponent<EnemyBullet>();
        bullet.AddMoveSpeed(bulletSpeedBonus);
        bullet.SetDirection((player.transform.position - muzzle.position).normalized);
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
}
