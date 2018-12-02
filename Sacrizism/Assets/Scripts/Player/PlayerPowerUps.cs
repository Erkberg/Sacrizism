using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    public const int healthGain = 1;
    public const int damageGain = 1;
    public const float moveSpeedGain = 0.5f;
    public const float reloadTimeGain = 0.1f;
    public const float bulletSizeGain = 0.1f;
    public const float bulletSpeedGain = 2f;
    public const int multishotGain = 1;
    public const int pierceGain = 1;

    public int bonusHealth = 0;
    public int bonusDamage = 0;
    public float bonusMoveSpeed = 0f;
    public float bonusReloadTime = 0f;
    public float bonusBulletSize = 0f;
    public float bonusBulletSpeed = 0f;
    public int bonusMultishot = 0;
    public int bonusPierce = 0;

    public void OnPowerUpPickedUp()
    {
        PowerUpType powerUpType = GetRandomPowerUpType();
        GameManager.instance.uiManager.OnPowerUpPickedUp(powerUpType.ToString());

        ApplyPowerUp(powerUpType);
    }

    private void ApplyPowerUp(PowerUpType powerUpType)
    {
        switch(powerUpType)
        {
            case PowerUpType.Health:
                bonusHealth += healthGain;
                GetComponent<Character>().Heal(100);
                break;

            case PowerUpType.Damage:
                bonusDamage += damageGain;
                break;

            case PowerUpType.MoveSpeed:
                bonusMoveSpeed += moveSpeedGain;
                break;

            case PowerUpType.ReloadTime:
                bonusReloadTime += reloadTimeGain;
                break;

            case PowerUpType.BulletSize:
                bonusBulletSize += bulletSizeGain;
                break;

            case PowerUpType.BulletSpeed:
                bonusBulletSpeed += bulletSpeedGain;
                break;

            case PowerUpType.Multishot:
                bonusMultishot += multishotGain;
                break;

            case PowerUpType.Pierce:
                bonusPierce += pierceGain;
                break;
        }
    }

    public PowerUpType GetRandomPowerUpType()
    {
        var values = System.Enum.GetValues(typeof(PowerUpType));
        return (PowerUpType)values.GetValue(Random.Range(0, values.Length));
    }

    public PowerUpOnceType GetRandomPowerUpOnceType()
    {
        var values = System.Enum.GetValues(typeof(PowerUpOnceType));
        return (PowerUpOnceType)values.GetValue(Random.Range(0, values.Length));
    }
}

public enum PowerUpType
{
    Health,
    Damage,
    MoveSpeed,
    ReloadTime,
    BulletSize,
    BulletSpeed,
    Multishot,
    Pierce
}

public enum PowerUpOnceType
{
    TunnelVision,
    Psychedelic,
    InBloom,
    CoolHat
}