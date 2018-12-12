using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public HPBar hpBar;

    public float maxHP = 3;
    private float currentHP;

    public Transform healOrigin;

    public UnityEvent onTakeDamage;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void SetMaxHP(int amount)
    {
        maxHP = amount;
        currentHP = maxHP;

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    public void TakeDamage(float amount)
    {
        onTakeDamage.Invoke();

        currentHP -= amount;        

        if(currentHP <= 0f)
        {
            Die();
        }

        if (gameObject.CompareTag(Tags.PlayerTag))
        {
            GameManager.instance.audioManager.PlayPlayerHurtSound();
        }
        else
        {
            GameManager.instance.enemyManager.OnAttackedEnemy();

            if (GetComponent<Enemy>().enemyType == EnemyType.Runner)
            {
                GameManager.instance.audioManager.PlayKachuHurtSound();
            }
            else
            {
                GameManager.instance.audioManager.PlayEnemyHurtSound();
            }
        }
            

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    public void Heal(float amount)
    {
        currentHP += amount;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        GameManager.instance.particlesManager.SpawnHealParticle(healOrigin);

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    public void Die()
    {
        if(gameObject.CompareTag(Tags.PlayerTag))
        {
            gameObject.SetActive(false);
            GameManager.instance.particlesManager.SpawnDeathParticle(transform);
            GameManager.instance.OnDeath();
        }
        else
        {
            if(GetComponent<Enemy>().hasPowerUp)
            {
                GameManager.instance.SpawnPowerUp(transform.position);
            }

            GameManager.instance.OnEnemyKilled(GetComponent<Enemy>().level);
            GameManager.instance.particlesManager.SpawnDeathParticle(transform);
            Destroy(gameObject);
        }
    }

    public bool IsInjured()
    {
        if(currentHP < maxHP && currentHP > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.LavaTag))
        {
            Die();
        }
    }
}
