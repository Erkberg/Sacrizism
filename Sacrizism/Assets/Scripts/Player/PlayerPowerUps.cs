using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    private const float onceChance = 0.1f;
    private List<PowerUpOnceType> onceTypesUsed = new List<PowerUpOnceType>();
    private static List<PowerUpOnceType> savedOnceTypesUsed = new List<PowerUpOnceType>();

    public int powerUpsCollected = 0;

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

    public static int savedBonusHealth = 0;
    public static int savedBonusDamage = 0;
    public static float savedBonusMoveSpeed = 0f;
    public static float savedBonusReloadTime = 0f;
    public static float savedBonusBulletSize = 0f;
    public static float savedBonusBulletSpeed = 0f;
    public static int savedBonusMultishot = 0;
    public static int savedBonusPierce = 0;

    public GameObject hat;

    public void OnPowerUpPickedUp()
    {
        if(Random.Range(0f, 1f) < onceChance)
        {
            PickOncePowerUp();
        }
        else
        {
            PickOrdinaryPowerUp();
        }
    }

    private void PickOrdinaryPowerUp()
    {
        PowerUpType powerUpType = GetRandomPowerUpType();

        string text = "!!! " + powerUpType.ToString() + " !!!";
        GameManager.instance.uiManager.OnPowerUpPickedUp(text);

        ApplyPowerUp(powerUpType);

        powerUpsCollected++;
    }

    private void PickOncePowerUp()
    {
        PowerUpOnceType powerUpType = GetRandomAvailablePowerUpOnceType();

        if (powerUpType == PowerUpOnceType.Unavailable)
        {
            PickOrdinaryPowerUp();
            return;
        }

        string text = "!!! " + powerUpType.ToString() + " !!!";
        GameManager.instance.uiManager.OnPowerUpPickedUp(text);
        onceTypesUsed.Add(powerUpType);

        ApplyPowerUpOnce(powerUpType);
    }

    private void ApplyPowerUp(PowerUpType powerUpType)
    {
        switch(powerUpType)
        {
            case PowerUpType.Health:
                bonusHealth += healthGain;
                GetComponent<Character>().maxHP += healthGain;
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

    private void ApplyPowerUpOnce(PowerUpOnceType powerUpType)
    {
        if(powerUpType == PowerUpOnceType.AwesomeHat)
        {
            hat.SetActive(true);
        }
        else
        {
            GameManager.instance.postProcessingManager.EnableVolume(powerUpType);
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
        return (PowerUpOnceType)values.GetValue(Random.Range(0, values.Length - 1));
    }

    public PowerUpOnceType GetRandomAvailablePowerUpOnceType()
    {
        PowerUpOnceType powerUpOnceType = GetRandomPowerUpOnceType();

        if(onceTypesUsed != null && onceTypesUsed.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                if (onceTypesUsed.Contains(powerUpOnceType))
                {
                    powerUpOnceType = GetRandomPowerUpOnceType();
                }
                else
                {
                    break;
                }
            }
        }

        if (onceTypesUsed.Contains(powerUpOnceType))
        {
            powerUpOnceType = PowerUpOnceType.Unavailable;
        }

        return powerUpOnceType;
    }

    public void SavePowerUps()
    {
        savedBonusHealth = bonusHealth;
        savedBonusDamage = bonusDamage ;
        savedBonusMoveSpeed = bonusMoveSpeed;
        savedBonusReloadTime = bonusReloadTime;
        savedBonusBulletSize = bonusBulletSize;
        savedBonusBulletSpeed = bonusBulletSpeed;
        savedBonusMultishot = bonusMultishot;
        savedBonusPierce = bonusPierce;

        savedOnceTypesUsed = new List<PowerUpOnceType>(onceTypesUsed);
    }

    public IEnumerator RestorePowerUps()
    {
        yield return null;

        bonusHealth = savedBonusHealth;
        bonusDamage = savedBonusDamage;
        bonusMoveSpeed = savedBonusMoveSpeed;
        bonusReloadTime = savedBonusReloadTime;
        bonusBulletSize = savedBonusBulletSize;
        bonusBulletSpeed = savedBonusBulletSpeed;
        bonusMultishot = savedBonusMultishot;
        bonusPierce = savedBonusPierce;

        GetComponent<Character>().maxHP += bonusHealth;
        GetComponent<Character>().Heal(100);

        onceTypesUsed = new List<PowerUpOnceType>(savedOnceTypesUsed);

        if(onceTypesUsed != null && onceTypesUsed.Count > 0)
        {
            foreach(PowerUpOnceType powerUpOnceType in onceTypesUsed)
            {
                ApplyPowerUpOnce(powerUpOnceType);
            }
        }

        savedOnceTypesUsed.Clear();

        savedBonusHealth = 0;
        savedBonusDamage = 0;
        savedBonusMoveSpeed = 0f;
        savedBonusReloadTime = 0f;
        savedBonusBulletSize = 0f;
        savedBonusBulletSpeed = 0f;
        savedBonusMultishot = 0;
        savedBonusPierce = 0;
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
    Noir,
    Film,
    AwesomeHat,
    Unavailable
}