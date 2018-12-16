using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform playerBullet;
    public Transform bulletsHolder;
    public Transform crosshair;

    public float reloadTime = 0.33f;
    public float shootingThreshold = 0.1f;
    public float recoilAmount = 0.1f;

    public ChangeFacing changeFacing;
    public PlayerPowerUps playerPowerUps;

    private float reloadTimePassed = 0f;
    private bool isReloading = false;

    public bool hasRecoil = false;
    public bool hasWobble = false;

    public bool shootingEnabled = true;

    private bool currentlyUsingMouse = false;
    private Vector3 previousMousePosition;
    private Camera mainCam;
    private readonly Vector3 camOffset = new Vector3(0f, 0f, 10f);

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update ()
    {
        CheckUsingMouse();

        if(currentlyUsingMouse)
        {
            MoveCrosshair();
        }

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
        if(currentlyUsingMouse)
        {
            if(crosshair.position.x < transform.position.x)
            {
                changeFacing.SetFacing(Facing.Left);
            }

            if (crosshair.position.x > transform.position.x)
            {
                changeFacing.SetFacing(Facing.Right);
            }
        }
        else
        {
            if (Input.GetAxis(InputConsts.HorizontalAimingAxis) < -shootingThreshold)
            {
                changeFacing.SetFacing(Facing.Left);
            }

            if (Input.GetAxis(InputConsts.HorizontalAimingAxis) > shootingThreshold)
            {
                changeFacing.SetFacing(Facing.Right);
            }
        }
    }

    private void MoveCrosshair()
    {
        crosshair.position = mainCam.ScreenToWorldPoint(previousMousePosition) + camOffset;
    }

    private void CheckUsingMouse()
    {
        if(currentlyUsingMouse)
        {
            if(Mathf.Abs(Input.GetAxis(InputConsts.HorizontalAimingAxis)) > shootingThreshold || 
                Mathf.Abs(Input.GetAxis(InputConsts.VerticalAimingAxis)) > shootingThreshold)
            {
                currentlyUsingMouse = false;
                crosshair.gameObject.SetActive(false);
            }
        }
        else
        {
            if(Input.GetMouseButton(0) || Input.mousePosition != previousMousePosition)
            {
                currentlyUsingMouse = true;
                crosshair.gameObject.SetActive(true);
            }
        }

        previousMousePosition = Input.mousePosition;
    }

    private void CheckShoot()
    {
        if(currentlyUsingMouse)
        {
            if(Input.GetMouseButton(0))
            {
                Shoot((crosshair.position - transform.position).normalized);
            }
        }
        else
        {
            Vector2 input = new Vector2(Input.GetAxis(InputConsts.HorizontalAimingAxis), Input.GetAxis(InputConsts.VerticalAimingAxis));

            if (input.sqrMagnitude > shootingThreshold)
            {
                Shoot(input.normalized);
            }
        }
    }

    private void Shoot(Vector2 direction)
    {
        isReloading = true;
        GameManager.instance.audioManager.PlayShootSound();

        GameManager.instance.cameraMovement.Shake(1.5f, 1.5f);

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

        if (hasRecoil)
        {
            transform.position -= (Vector3)direction * recoilAmount;
        }
    }

    private void SpawnBullet(Vector2 direction)
    {
        PlayerBullet bullet = Instantiate(playerBullet, transform.position, Quaternion.identity, bulletsHolder).GetComponent<PlayerBullet>();
        bullet.AddDamage(playerPowerUps.bonusDamage);
        bullet.AddSize(playerPowerUps.bonusBulletSize);
        bullet.AddMoveSpeed(playerPowerUps.bonusBulletSpeed);
        bullet.AddPierce(playerPowerUps.bonusPierce);
        bullet.SetWobbling(hasWobble);
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
