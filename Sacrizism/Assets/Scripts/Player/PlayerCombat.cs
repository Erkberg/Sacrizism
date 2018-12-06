using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform playerBullet;
    public Transform bulletsHolder;

    public float reloadTime = 0.33f;
    public float shootingThreshold = 0.1f;

    public ChangeFacing changeFacing;
    public PlayerPowerUps playerPowerUps;

    private float reloadTimePassed = 0f;
    private bool isReloading = false;

    public bool shootingEnabled = true;
	
	// Update is called once per frame
	void Update ()
    {
		if(isReloading)
        {
            Reload();
        }
        else
        {
            if(shootingEnabled)
            {
                CheckShoot();
            }            
        }
        //Debug.DrawRay(transform.position, new Vector2(Input.GetAxis(InputConsts.HorizontalAimingAxis), Input.GetAxis(InputConsts.VerticalAimingAxis)));
        CheckFacing();
	}

    private void CheckFacing()
    {
        if(Input.GetAxis(InputConsts.HorizontalAimingAxis) < -shootingThreshold)
        {
            changeFacing.SetFacing(Facing.Left);
        }

        if (Input.GetAxis(InputConsts.HorizontalAimingAxis) > shootingThreshold)
        {
            changeFacing.SetFacing(Facing.Right);
        }
    }

    private void CheckShoot()
    {
        Vector2 input = new Vector2(Input.GetAxis(InputConsts.HorizontalAimingAxis), Input.GetAxis(InputConsts.VerticalAimingAxis));

        if(input.sqrMagnitude > shootingThreshold)
        {
            Shoot(input.normalized);
        }
    }

    private void Shoot(Vector2 direction)
    {
        isReloading = true;
        GameManager.instance.audioManager.PlayShootSound();

        GameManager.instance.cameraMovement.Shake();

        SpawnBullet(direction);

        int amountOfMultishots = playerPowerUps.bonusMultishot;
        if (amountOfMultishots > 0)
        {
            float maxAngle = amountOfMultishots * 30f;

            for(int i = 0; i < amountOfMultishots; i++)
            {
                float degrees = Random.Range(-maxAngle, maxAngle);
                Vector2 newDirection = Quaternion.Euler(0, 0, degrees) * direction;
                SpawnBullet(newDirection);
            }
        }
    }

    private void SpawnBullet(Vector2 direction)
    {
        PlayerBullet bullet = Instantiate(playerBullet, transform.position, Quaternion.identity, bulletsHolder).GetComponent<PlayerBullet>();
        bullet.AddDamage(playerPowerUps.bonusDamage);
        bullet.AddSize(playerPowerUps.bonusBulletSize);
        bullet.AddMoveSpeed(playerPowerUps.bonusBulletSpeed);
        bullet.AddPierce(playerPowerUps.bonusPierce);
        bullet.SetDirection(direction);
    }

    private void Reload()
    {
        reloadTimePassed += Time.deltaTime;

        if(reloadTimePassed >= (reloadTime - playerPowerUps.bonusReloadTime))
        {
            isReloading = false;
            reloadTimePassed = 0f;
        }
    }
}
