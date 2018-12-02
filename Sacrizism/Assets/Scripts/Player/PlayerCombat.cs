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
	
	// Update is called once per frame
	void Update ()
    {
		if(isReloading)
        {
            Reload();
        }
        else
        {
            CheckShoot();
        }

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

        GameManager.instance.cameraMovement.Shake();

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
